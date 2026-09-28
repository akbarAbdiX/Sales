using System;
using System.Linq;
using BigBang.App.Sales.Common.Exceptions;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.DataAccess;
using static BigBang.App.Sales.Common.Constants.SaleConstant;

namespace BigBang.App.Sales.Service.SOP.Concrete
{
    [InternalService(ServiceType = typeof(IDocumentAdjustmentService), InstanceMode = InstanceMode.Scoped)]
    internal class DocumentAdjustmentService : IDocumentAdjustmentService
    {
        private readonly IAdjustmentRepository _adjustmentRepository;
        private readonly IDocumentAdjustmentRepository _documentAdjustmentRepository;
        private readonly IDocumentItemRepository _documentItemRepository;
        private readonly IDocumentRepository _documentRepository;

        public DocumentAdjustmentService(IAdjustmentRepository adjustmentRepository,
                                         IDocumentAdjustmentRepository documentAdjustmentRepository,
                                         IDocumentItemRepository documentItemRepository,
                                         IDocumentRepository documentRepository)
        {
            _adjustmentRepository = adjustmentRepository;
            _documentAdjustmentRepository = documentAdjustmentRepository;
            _documentItemRepository = documentItemRepository;
            _documentRepository = documentRepository;
        }

        private void EnsureDocumentIsModifiable(int documentId)
        {
            var document = _documentRepository.GetFirstOrDefault(d => d.ID == documentId);
            if (document == null || document.StateID != Domain.Enums.SOPChangeStateHistoryEntityStateType.Draft)
                throw Exceptions.Raise(ExceptionCode.DocumentStateIsInvalid);
        }
        private static DateTime? GetEffectiveDocumentDate(SALSOPDocument document)
        {
            if (document == null)
                return null;

            if (document.StartDate.HasValue)
                return document.StartDate;

            return document.IssueDate != default(DateTime)
                ? (DateTime?)document.IssueDate
                : null;
        }

        private static bool IsAdjustmentApplicable(DateTime? effectiveDocumentDate, DateTime? fromDate, DateTime? toDate)
        {
            if (!effectiveDocumentDate.HasValue)
                return true;

            if (fromDate.HasValue && effectiveDocumentDate.Value < fromDate.Value)
                return false;

            if (toDate.HasValue && effectiveDocumentDate.Value > toDate.Value)
                return false;

            return true;
        }

        public void DocumentAdjustmentOnAdding(SALSOPDocumentAdjustment entity)
        {
            var newRow = entity;
            if (newRow?.DocumentItem == null || newRow.Adjustment == null)
                return;


            EnsureDocumentIsModifiable(newRow.DocumentItem.Document.ID);

            var adjustment = _adjustmentRepository.GetFirstOrDefault(x => x.ID == newRow.Adjustment.ID);
            if (adjustment == null) return;

            var changeType = GetChangeType(adjustment.AdjustmentType?.Name);
            if (changeType == ChangeType.Default) return;

            var item = _documentItemRepository.GetFirstOrDefault(x => x.ID == newRow.DocumentItem.ID);
            if (item == null) return;

            var docId = item.Document.ID;

            var document = _documentRepository.GetFirstOrDefault(d => d.ID == docId);
            if (document == null) return;

            var effectiveDate = GetEffectiveDocumentDate(document);

            var sums = _documentAdjustmentRepository.GetMany(a => a.DocumentItem.ID == item.ID)
                .Select(a => new {
                    a.AdjustmentAmount,
                    TypeName = a.Adjustment.AdjustmentType.Name,
                    a.Adjustment.FromDate,
                    a.Adjustment.ToDate
                })
                .ToList()
                .Where(s => IsAdjustmentApplicable(effectiveDate, s.FromDate, s.ToDate))
                .ToList();

            decimal sumDiscount = sums.Where(s => s.TypeName.Equals(Discount, StringComparison.OrdinalIgnoreCase)).Sum(s => s.AdjustmentAmount);
            decimal sumTax = sums.Where(s => s.TypeName.Equals(Tax, StringComparison.OrdinalIgnoreCase)).Sum(s => s.AdjustmentAmount);
            decimal sumAdditional = sums.Where(s => s.TypeName.Equals(AdditionalCharges, StringComparison.OrdinalIgnoreCase)).Sum(s => s.AdjustmentAmount);

            if (IsAdjustmentApplicable(effectiveDate, adjustment.FromDate, adjustment.ToDate))
            {
                var amount = newRow.AdjustmentAmount;

                switch (changeType)
                {
                    case ChangeType.Discount: sumDiscount += amount; break;
                    case ChangeType.Tax: sumTax += amount; break;
                    case ChangeType.AdditionalCharge: sumAdditional += amount; break;
                }
            }

            item.Discount = sumDiscount;
            item.Tax = sumTax;
            item.AdditionalCharge = sumAdditional;

            item.TotalPrice = (item.Quantity * item.UnitPrice) - item.Discount + item.Tax + item.AdditionalCharge;
            item.EntityState = Metadata.Models.EntityState.Modified;

            _documentItemRepository.Update(item);

            var aggregates = _documentItemRepository
                .GetMany(i => i.Document.ID == docId)
                .Select(i => new {
                    SubTotal = (decimal?)i.Quantity * i.UnitPrice,
                    Discount = (decimal?)i.Discount,
                    Tax = (decimal?)i.Tax,
                    Additional = (decimal?)i.AdditionalCharge
                })
                .ToList();

            decimal subTotalAgg = aggregates.Sum(x => x.SubTotal ?? 0m);
            decimal discountAgg = aggregates.Sum(x => x.Discount ?? 0m);
            decimal taxAgg = aggregates.Sum(x => x.Tax ?? 0m);
            decimal additionalAgg = aggregates.Sum(x => x.Additional ?? 0m);
            decimal net = subTotalAgg - discountAgg + taxAgg + additionalAgg;

            document.DiscountAmount = discountAgg;
            document.TaxAmount = taxAgg;
            document.AdditionalChargesAmount = additionalAgg;
            document.TotalAmount = net;

            _documentRepository.Update(document);
        }

        public void DocumentAdjustmentOnUpdating(SALSOPDocumentAdjustment entity)
        {
            var documentAdjustment = entity;
            if (documentAdjustment?.DocumentItem == null || documentAdjustment.Adjustment == null)
                return;


            EnsureDocumentIsModifiable(documentAdjustment.DocumentItem.Document.ID);

            var documentItem = _documentItemRepository.GetFirstOrDefault(x => x.ID == documentAdjustment.DocumentItem.ID);
            if (documentItem == null) return;

            var documentId = documentItem.Document.ID;
            var effectiveDate = GetEffectiveDocumentDate(documentItem.Document);

            var adjustmentSums = _documentAdjustmentRepository
                .GetMany(a => a.DocumentItem.ID == documentItem.ID)
                .Select(a => new {
                    a.AdjustmentAmount,
                    TypeName = a.Adjustment.AdjustmentType.Name,
                    a.Adjustment.FromDate,
                    a.Adjustment.ToDate
                })
                .ToList()
                .Where(s => IsAdjustmentApplicable(effectiveDate, s.FromDate, s.ToDate))
                .ToList();

            decimal discountSum = adjustmentSums.Where(s => s.TypeName.Equals(Discount, StringComparison.OrdinalIgnoreCase)).Sum(s => s.AdjustmentAmount);
            decimal taxSum = adjustmentSums.Where(s => s.TypeName.Equals(Tax, StringComparison.OrdinalIgnoreCase)).Sum(s => s.AdjustmentAmount);
            decimal additionalSum = adjustmentSums.Where(s => s.TypeName.Equals(AdditionalCharges, StringComparison.OrdinalIgnoreCase)).Sum(s => s.AdjustmentAmount);

            documentItem.Discount = discountSum;
            documentItem.Tax = taxSum;
            documentItem.AdditionalCharge = additionalSum;

            documentItem.TotalPrice = (documentItem.Quantity * documentItem.UnitPrice) - documentItem.Discount + documentItem.Tax + documentItem.AdditionalCharge;
            documentItem.EntityState = Metadata.Models.EntityState.Modified;

            ServiceLocator.Get<IBulkRepository<SALSOPDocumentItem>>()
                .Builder
                .Entities(new[] { documentItem })
                .AllowedProperties(i => i.Discount, i => i.Tax, i => i.AdditionalCharge, i => i.TotalPrice, i => i.Description)
                .Save();

            var aggregates = _documentItemRepository
                .GetMany(i => i.Document.ID == documentId)
                .Select(i => new {
                    SubTotal = (decimal?)i.Quantity * i.UnitPrice,
                    Discount = (decimal?)i.Discount,
                    Tax = (decimal?)i.Tax,
                    Additional = (decimal?)i.AdditionalCharge
                })
                .ToList();

            decimal subTotal = aggregates.Sum(x => x.SubTotal ?? 0m);
            decimal totalDiscount = aggregates.Sum(x => x.Discount ?? 0m);
            decimal totalTax = aggregates.Sum(x => x.Tax ?? 0m);
            decimal totalAdd = aggregates.Sum(x => x.Additional ?? 0m);
            decimal net = subTotal - totalDiscount + totalTax + totalAdd;

            var document = new SALSOPDocument
            {
                ID = documentId,
                DiscountAmount = totalDiscount,
                TaxAmount = totalTax,
                AdditionalChargesAmount = totalAdd,
                TotalAmount = net,
                EntityState = Metadata.Models.EntityState.Modified
            };

            ServiceLocator.Get<IBulkRepository<SALSOPDocument>>()
                .Builder
                .Entities(new[] { document })
                .AllowedProperties(
                    d => d.DiscountAmount,
                    d => d.TaxAmount,
                    d => d.AdditionalChargesAmount,
                    d => d.TotalAmount
                )
                .Save();
        }

        public void DocumentAdjustmentOnDeleting(SALSOPDocumentAdjustment entity)
        {
            var adjustmentDocument = entity;
            if (adjustmentDocument == null || adjustmentDocument.DocumentItem == null || adjustmentDocument.Adjustment == null)
                return;


            EnsureDocumentIsModifiable(adjustmentDocument.DocumentItem.Document.ID);

            var documentItem = adjustmentDocument.DocumentItem;
            var document = documentItem.Document;

            var adjustment = _adjustmentRepository.GetFirstOrDefault(x => x.ID == adjustmentDocument.Adjustment.ID)
                              ?? adjustmentDocument.Adjustment;

            var effectiveDate = GetEffectiveDocumentDate(document);

            if (!IsAdjustmentApplicable(effectiveDate, adjustment.FromDate, adjustment.ToDate))
            {
                return;
            }

            if (adjustmentDocument.Adjustment.AdjustmentType.Name == Discount)
            {
                documentItem.Discount -= adjustmentDocument.AdjustmentAmount;
                documentItem.TotalPrice += adjustmentDocument.AdjustmentAmount;
                document.DiscountAmount -= adjustmentDocument.AdjustmentAmount;
                document.TotalAmount += adjustmentDocument.AdjustmentAmount;
            }
            else if (adjustmentDocument.Adjustment.AdjustmentType.Name == Tax)
            {
                documentItem.Tax -= adjustmentDocument.AdjustmentAmount;
                documentItem.TotalPrice -= adjustmentDocument.AdjustmentAmount;
                document.TaxAmount -= adjustmentDocument.AdjustmentAmount;
                document.TotalAmount -= adjustmentDocument.AdjustmentAmount;
            }
            else if (adjustmentDocument.Adjustment.AdjustmentType.Name == AdditionalCharges)
            {
                documentItem.AdditionalCharge -= adjustmentDocument.AdjustmentAmount;
                documentItem.TotalPrice -= adjustmentDocument.AdjustmentAmount;
                document.AdditionalChargesAmount -= adjustmentDocument.AdjustmentAmount;
                document.TotalAmount -= adjustmentDocument.AdjustmentAmount;
            }

            documentItem.EntityState = Metadata.Models.EntityState.Modified;
            document.EntityState = Metadata.Models.EntityState.Modified;

            _documentItemRepository.Update(documentItem);
            _documentRepository.Update(document);
        }

        private static ChangeType GetChangeType(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return ChangeType.Default;
            name = name.Trim();

            if (name.Equals(Discount, StringComparison.OrdinalIgnoreCase)) return ChangeType.Discount;
            if (name.Equals(Tax, StringComparison.OrdinalIgnoreCase)) return ChangeType.Tax;
            return name.Equals(AdditionalCharges, StringComparison.OrdinalIgnoreCase) ? ChangeType.AdditionalCharge : ChangeType.Default;
        }
    }
}