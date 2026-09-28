using System;
using System.Collections.Generic;
using System.Linq;
using BigBang.App.Sales.Common.Exceptions;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.DataAccess;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;
using Polly;
using static BigBang.App.Sales.Common.Constants.SaleConstant;

namespace BigBang.App.Sales.Service.SOP.Concrete
{
    [InternalService(ServiceType = typeof(IDocumentItemService), InstanceMode = InstanceMode.Scoped)]
    internal class DocumentItemService : IDocumentItemService
    {
        private readonly IAdjustmentRepository _adjustmentRepository;
        private readonly IDocumentAdjustmentRepository _documentAdjustmentRepository;
        private readonly IDocumentItemRepository _documentItemRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentService _documentService;
        private readonly ISession _session;

        public DocumentItemService(IAdjustmentRepository adjustmentRepository,
                                   IDocumentAdjustmentRepository documentAdjustmentRepository,
                                   IDocumentItemRepository documentItemRepository,
                                   IDocumentRepository documentRepository,
                                   IDocumentService documentService,
                                   ISession session)
        {
            _adjustmentRepository = adjustmentRepository;
            _documentAdjustmentRepository = documentAdjustmentRepository;
            _documentItemRepository = documentItemRepository;
            _documentRepository = documentRepository;
            _documentService = documentService;
            _session = session;
        }

        private void EnsureDocumentIsModifiable(int documentId)
        {
            //var document = _documentRepository.GetFirstOrDefault(d => d.ID == documentId);
            //if (document == null || document.StateID != null)
            //    throw Exceptions.Raise(ExceptionCode.DocumentStateIsInvalid);
        }

        private bool IsTaxExempt(int productDetailId)
        {
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);

            return _session.Query<SALSOPProductDetailTaxExemption>()
                .Any(w => w.ProductDetailId.Id == productDetailId
                       && w.FromDate < tomorrow
                       && (w.ToDate == null || w.ToDate >= today));
        }

        public void DocumentItemOnAdding(SALSOPDocumentItem entity)
        {
            EnsureDocumentIsModifiable(entity.Document.ID);
            var item = entity;
            var docId = item.Document.ID;

            bool taxExemption = IsTaxExempt(item.ItemID.Id);

            var defaultAdjustments = _adjustmentRepository
                .GetMany(x => x.IsDefault)
                .ToList();

            var baseAmount = item.Quantity * item.UnitPrice;
            var createdAdjustments = new List<SALSOPDocumentAdjustment>();

            foreach (var adjustment in defaultAdjustments)
            {
                if (taxExemption && adjustment.AdjustmentType?.Name.Equals(Tax, StringComparison.OrdinalIgnoreCase) == true)
                    continue;

                var adjAmount = (adjustment.Amount == 0m && adjustment.Rate != 0m)
                    ? (baseAmount * adjustment.Rate) / 100m
                    : adjustment.Amount;

                var docAdj = new SALSOPDocumentAdjustment
                {
                    DocumentItem = item,
                    Adjustment = new SALSOPAdjustment { ID = adjustment.ID, AdjustmentType = adjustment.AdjustmentType },
                    AdjustmentAmount = adjAmount,
                    ParameterAmount = 0m
                };

                _documentAdjustmentRepository.Insert(docAdj);
                createdAdjustments.Add(docAdj);
            }

            UpdateItemAndDocumentAggregates(item, createdAdjustments);
        }

        public void DocumentItemOnUpdating(SALSOPDocumentItem entity)
        {
            EnsureDocumentIsModifiable(entity.Document.ID);
            var item = entity;

            bool taxExemption = IsTaxExempt(item.ItemID.Id);
            var baseAmount = item.Quantity * item.UnitPrice;

            var docAdjustments = _documentAdjustmentRepository.GetMany(x => x.DocumentItem.ID == item.ID).ToList();

            var defaultAdjustments = _adjustmentRepository.GetMany(x => x.IsDefault).ToList();

            foreach (var defAdj in defaultAdjustments)
            {
                var existingAdj = docAdjustments.FirstOrDefault(a => a.Adjustment.ID == defAdj.ID);
                bool isTaxType = defAdj.AdjustmentType?.Name.Equals(Tax, StringComparison.OrdinalIgnoreCase) == true;

                if (isTaxType && taxExemption)
                {
                    if (existingAdj != null)
                    {
                        existingAdj.AdjustmentAmount = 0m;
                        _documentAdjustmentRepository.Update(existingAdj);
                    }
                    continue;
                }

                decimal newAdjAmount = (defAdj.Amount == 0m && defAdj.Rate != 0m)
                                     ? (baseAmount * defAdj.Rate) / 100m
                                     : defAdj.Amount;

                if (existingAdj != null)
                {
                    existingAdj.AdjustmentAmount = newAdjAmount;
                    existingAdj.ParameterAmount = 0m;
                    _documentAdjustmentRepository.Update(existingAdj);
                }
                else
                {
                    var newDocAdj = new SALSOPDocumentAdjustment
                    {
                        DocumentItem = item,
                        Adjustment = defAdj,
                        AdjustmentAmount = newAdjAmount,
                        ParameterAmount = 0m
                    };
                    _documentAdjustmentRepository.Insert(newDocAdj);
                    docAdjustments.Add(newDocAdj);
                }
            }

            item.EntityState = Metadata.Models.EntityState.Modified;

            UpdateItemAndDocumentAggregates(item, docAdjustments, true);
        }

        public void DocumentItemOnDeleting(SALSOPDocumentItem entity)
        {
            if (entity?.Document == null) return;
            EnsureDocumentIsModifiable(entity.Document.ID);

            var item = entity;
            var adjustments = _documentAdjustmentRepository
                                    .GetMany(a => a.DocumentItem.ID == item.ID)
                                    .ToList();

            foreach (var adjustment in adjustments)
            {
                _documentAdjustmentRepository.Delete(adjustment);
            }

            UpdateDocumentAggregatesOnly(item.Document.ID, item.ID);
        }

        public SALSOPDocumentItem GetById(int Id)
        {
            return _documentItemRepository.GetFirstOrDefault(x => x.ID == Id);
        }

        public bool ValidateToUpdate(SALSOPDocumentItem entity)
        {
            var oldEntity = _documentItemRepository.GetFirstOrDefault(x => x.ID == entity.ID);
            return entity.ItemID.Id != oldEntity.ItemID.Id;
        }

        /// <summary>
        /// چک کردن یونیک بودن یک قلم آیتم به ازاء هر سند فارغ از نوع سند
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="BigBangException"></exception>
        public void ValidateItemIsUniquePerType(SALSOPDocumentItem entity)
        {
            if (entity.Document == null)
                return;

            var document = _documentService.GetByID(entity.Document.ID);
            if (document == null /*در حال ایجاد آیتم می باشد*/)
                return;

            var item = document.SOPDocumentItems.FirstOrDefault(x => x.ItemID.Id == entity.ItemID.Id);/*from database*/
            if (item != null)/*یعنی از قبل یک آیتم با این عنوان برای این سند تعریف شده است.*/
            {
                throw new BigBangException($"برای سند جاری نمی توان بیش از یک آیتم « {entity.ItemID.ProductDetailName} » تعریف کرد.");
            }
        }

        private void UpdateItemAndDocumentAggregates(SALSOPDocumentItem item, List<SALSOPDocumentAdjustment> docAdjustments, bool isUpdate = false)
        {
            decimal sumDiscount = 0m, sumTax = 0m, sumAdditional = 0m;

            foreach (var adj in docAdjustments)
            {
                var typeName = adj?.Adjustment?.AdjustmentType?.Name ?? string.Empty;
                if (typeName.Equals(Discount, StringComparison.OrdinalIgnoreCase))
                    sumDiscount += adj.AdjustmentAmount;
                else if (typeName.Equals(Tax, StringComparison.OrdinalIgnoreCase))
                    sumTax += adj.AdjustmentAmount;
                else if (typeName.Equals(AdditionalCharges, StringComparison.OrdinalIgnoreCase))
                    sumAdditional += adj.AdjustmentAmount;
            }

            item.Discount = sumDiscount;
            item.Tax = sumTax;
            item.AdditionalCharge = sumAdditional;
            item.TotalPrice = (item.Quantity * item.UnitPrice) - item.Discount + item.Tax + item.AdditionalCharge;

            if (isUpdate)
            {
                ServiceLocator.Get<IBulkRepository<SALSOPDocumentItem>>()
                    .Builder.Entities(new[] { item })
                    .AllowedProperties(i => i.Discount, i => i.Tax, i => i.AdditionalCharge, i => i.TotalPrice, i => i.Description)
                    .Save();
            }
            else
            {
                _documentItemRepository.Update(item);
            }

            UpdateDocumentAggregatesOnly(item.Document.ID, null, item, isUpdate);
        }

        private void UpdateDocumentAggregatesOnly(int docId, int? excludedItemId = null, SALSOPDocumentItem memoryItem = null, bool isUpdate = false)
        {
            var aggregatesQuery = _documentItemRepository.GetMany(i => i.Document.ID == docId);
            if (excludedItemId.HasValue)
            {
                aggregatesQuery = aggregatesQuery.Where(i => i.ID != excludedItemId.Value).ToList();
            }

            var aggregates = aggregatesQuery
                .Select(i => new {
                    i.ID,
                    SubTotal = (decimal?)i.Quantity * i.UnitPrice,
                    Discount = (decimal?)i.Discount,
                    Tax = (decimal?)i.Tax,
                    Additional = (decimal?)i.AdditionalCharge
                }).ToList();

            if (memoryItem != null)
            {
                aggregates.RemoveAll(x => x.ID == memoryItem.ID);
                aggregates.Add(new {
                    ID = memoryItem.ID,
                    SubTotal = (decimal?)(memoryItem.Quantity * memoryItem.UnitPrice),
                    Discount = (decimal?)memoryItem.Discount,
                    Tax = (decimal?)memoryItem.Tax,
                    Additional = (decimal?)memoryItem.AdditionalCharge
                });
            }

            decimal subTotalAgg = aggregates.Sum(x => x.SubTotal ?? 0m);
            decimal discountAgg = aggregates.Sum(x => x.Discount ?? 0m);
            decimal taxAgg = aggregates.Sum(x => x.Tax ?? 0m);
            decimal additionalAgg = aggregates.Sum(x => x.Additional ?? 0m);
            decimal net = subTotalAgg - discountAgg + taxAgg + additionalAgg;

            if (isUpdate)
            {
                var doc = new SALSOPDocument
                {
                    ID = docId,
                    DiscountAmount = discountAgg,
                    TaxAmount = taxAgg,
                    AdditionalChargesAmount = additionalAgg,
                    TotalAmount = net,
                    EntityState = Metadata.Models.EntityState.Modified,
                    Amount = subTotalAgg
                };

                ServiceLocator.Get<IBulkRepository<SALSOPDocument>>()
                    .Builder.Entities(new[] { doc })
                    .AllowedProperties(d => d.DiscountAmount, d => d.TaxAmount, d => d.AdditionalChargesAmount, d => d.TotalAmount, d => d.Amount)
                    .Save();
            }
            else
            {
                var document = _documentRepository.GetFirstOrDefault(d => d.ID == docId);
                if (document != null)
                {
                    document.DiscountAmount = discountAgg;
                    document.TaxAmount = taxAgg;
                    document.AdditionalChargesAmount = additionalAgg;
                    document.TotalAmount = net;
                    document.Amount = subTotalAgg;
                    _documentRepository.Update(document);
                }
            }
        }

    }
}
