using System;
using System.Collections.Generic;
using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;
using BigBang.App.Sales.Service.Role;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.DataAccess;
using BigBang.WebServer.Common.Exceptions;
using Microsoft.CodeAnalysis;
using NHibernate;
using NHibernate.Criterion;
using static BigBang.App.Sales.Common.Constants.SaleConstant;

namespace BigBang.App.Sales.Businesses.SOPDocument
{
    [BusinessService(typeof(SALSOPDocument))]
    public class SOPDocumentBusinessService
    {
        private decimal RoundToScale(decimal value, int decimalPlaces = 2)
        {
            return Math.Round(value, decimalPlaces, MidpointRounding.ToPositiveInfinity);
        }

        private readonly ISession _session;
        private readonly ICheckAccess _checkAccess;
        private readonly IDocumentService _documentService;
        private readonly IChangeStateHistoryService _changeStateHistoryService;


        public SOPDocumentBusinessService()
        {

        }

        public SOPDocumentBusinessService(ISession session,
                                          ICheckAccess checkAccess,
                                          IDocumentService documentService,
                                          IChangeStateHistoryService changeStateHistoryService)
        {
            _checkAccess = checkAccess;
            _session = session;
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
            _changeStateHistoryService = changeStateHistoryService ?? throw new ArgumentNullException(nameof(changeStateHistoryService));
        }

        public virtual void SetDocumentToSent(SALSOPDocument document)
        {
            _checkAccess.CheckDocumentsAccess("CheckAccessToChangeFlowPreInvoice", "SalesFinance", true);
            document.Color = null;
            ValidateDocument(document);
            document.StateID = Domain.Enums.SOPChangeStateHistoryEntityStateType.Sent;

            _changeStateHistoryService.Add((SOPChangeStateHistoryEntityType)document.DocumentType.Code,
                                           document.ID,
                                           (SOPChangeStateHistoryEntityStateType)document.StateID,
                                           "");
        }

        public virtual void SetDocumentToConfirmed(SALSOPDocument document)
        {
            _checkAccess.CheckDocumentsAccess("CheckAccessToChangeFlowPreInvoice", "SalesManagment", true);
            _checkAccess.CheckDocumentsAccess("CheckAccessToChangeFlowPreInvoice", "SalesFinance", true);
            document.Color = null;
            ValidateDocument(document);
            document.StateID = Domain.Enums.SOPChangeStateHistoryEntityStateType.Confirmed;

            _changeStateHistoryService.Add((SOPChangeStateHistoryEntityType)document.DocumentType.Code,
                                           document.ID,
                                           (SOPChangeStateHistoryEntityStateType)document.StateID,
                                           "");
        }

        public virtual void SetDocumentToApprovedBySalesManagment(SALSOPDocument document)
        {
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToChangeFlowPreInvoice", "SalesRepresentativ", true, "امکان تایید توسط مدیر فروش برای نقش کارشناس فروش وجود ندارد");
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToChangeFlowPreInvoice", "SalesFinance", true, "امکان تایید توسط مدیر فروش برای نقش کارشناس مالی وجود ندارد");

            document.Color = null;
            document.StateID = Domain.Enums.SOPChangeStateHistoryEntityStateType.ApprovedBySalesManagment;
            ValidateDocument(document);

            _changeStateHistoryService.Add((SOPChangeStateHistoryEntityType)document.DocumentType.Code,
                                           document.ID,
                                           (SOPChangeStateHistoryEntityStateType)document.StateID,
                                           "");
        }

        public virtual void SetDocumentToDiscarded(SALSOPDocument document)
        {
            document.Color = null;
            ValidateDocument(document);
            document.StateID = Domain.Enums.SOPChangeStateHistoryEntityStateType.Discarded;

            _changeStateHistoryService.Add((SOPChangeStateHistoryEntityType)document.DocumentType.Code,
                                           document.ID,
                                           (SOPChangeStateHistoryEntityStateType)document.StateID,
                                           "");
        }

        public virtual void SetDocumentToReject(SALSOPDocument document, string rejectionReason)
        {
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToChangeFlowPreInvoice", "SalesRepresentativ", true, "امکان عدم تایید برای نقش کارشناس فروش وجود ندارد");
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToChangeFlowPreInvoice", "SalesManagment", true, "امکان عدم تایید برای نقش مدیر فروش وجود ندارد");

            ValidateDocument(document);
            document.Color = 1;
            document.StateID = Domain.Enums.SOPChangeStateHistoryEntityStateType.Rejected;
            document.RejectionReason = rejectionReason;

            _changeStateHistoryService.Add((SOPChangeStateHistoryEntityType)document.DocumentType.Code,
                                           document.ID,
                                           (SOPChangeStateHistoryEntityStateType)document.StateID,
                                           document.RejectionReason);
        }

        public virtual void SetDocumentToRejectedBySalesManagment(SALSOPDocument document, string rejectionReason)
        {
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToChangeFlowPreInvoice", "SalesRepresentativ", true, "امکان تایید توسط مدیر فروش برای نقش کارشناس فروش وجود ندارد");
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToChangeFlowPreInvoice", "SalesFinance", true, "امکان تایید توسط مدیر فروش برای نقش کارشناس مالی وجود ندارد");

            document.Color = 1;
            document.StateID = Domain.Enums.SOPChangeStateHistoryEntityStateType.RejectedBySalesManagment;
            document.RejectionReason = rejectionReason;

            ValidateDocument(document);

            _changeStateHistoryService.Add((SOPChangeStateHistoryEntityType)document.DocumentType.Code,
                                           document.ID,
                                           (SOPChangeStateHistoryEntityStateType)document.StateID,
                                           document.RejectionReason);
        }

        public virtual void SetDocumentToApprove(SALSOPDocument document)
        {
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToChangeFlowPreInvoice", "SalesRepresentativ", true, "امکان تایید برای نقش کارشناس فروش وجود ندارد");
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToChangeFlowPreInvoice", "SalesManagment", true, "امکان تایید برای نقش مدیر فروش وجود ندارد");
            document.Color = null;
            document.StateID = Domain.Enums.SOPChangeStateHistoryEntityStateType.Approved;
            ValidateDocument(document);

            _changeStateHistoryService.Add((SOPChangeStateHistoryEntityType)document.DocumentType.Code,
                                           document.ID,
                                           (SOPChangeStateHistoryEntityStateType)document.StateID,
                                           "");
        }

        public virtual void Transition(SALSOPDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document), "سند ورودی نامعتبر (Null) است.");
            }

            if (_documentService.IsDefinedNewDocument(document))
            {
                throw new BigBangException($"سندی با شناسه {document.ID} قبلا تعریف شده است، لذا مجاز به تعریف مجدد آن نمی باشید.");
            }

            var currentDocument = _session.Get<SALSOPDocument>(document.ID);
            if (currentDocument == null)
            {
                throw new BigBangException($"سندی با شناسه {document.ID} یافت نشد.");
            }

            switch (document.DocumentType.Code)
            {
                case 1/*1   پیش فاکتور  ProformaInvoice*/:
                case 2/*2   سفارش Order*/:
                case 3/*3   قرارداد Contract*/:
                case 6/*6   پیشنهاد قیمت    PriceProposal*/:

                    if (currentDocument.StateID != SOPChangeStateHistoryEntityStateType.Confirmed)
                        throw new BigBangException($"سند با شناسه {document.ID} در وضعیت « تایید » قرار ندارد و امکان انتقال به مرحله بعد را ندارد.");

                    break;

                case 5/*5   درخواست صدور فاکتور InvoiceIssuanceRequest*/:

                    if (currentDocument.StateID != SOPChangeStateHistoryEntityStateType.Approved)
                        throw new BigBangException($"سند با شناسه {document.ID} در وضعیت « تایید » قرار ندارد و امکان انتقال به مرحله بعد را ندارد.");

                    break;
            }

            var currentFlow = _session.QueryOver<SALSOPDocumentFlow>()
                                      .Where(f => f.DocumentType.ID == currentDocument.DocumentType.ID)
                                      .SingleOrDefault();

            int currentLevel = currentFlow?.Level ?? 0;

            var nextTypeId = _session.QueryOver<SALSOPDocumentFlow>()
                                     .Where(f => f.Level > currentLevel)
                                     .OrderBy(f => f.Level).Asc
                                     .Select(Projections.Property<SALSOPDocumentFlow>(f => f.DocumentType.ID))
                                     .Take(1)
                                     .SingleOrDefault<int>();

            if (nextTypeId == 0)
            {
                throw new BigBangException($"بعد از سطح {currentLevel}، مرحله بالاتری در گردش کار سند تعریف نشده است.");
            }

            var nextDocumentType = _session.Get<SALSOPDocumentType>(nextTypeId);

            bool isNextTypeInvoice = nextDocumentType != null && nextDocumentType.Name == "Invoice";

            if (isNextTypeInvoice)
            {
                bool hasDraftInvoice = _session.QueryOver<SALSOPDocument>()
                                               .Where(d => d.DocumentType.ID == nextTypeId)
                                               .And(d => d.StateID == SOPChangeStateHistoryEntityStateType.Draft)
                                               .RowCount() > 0;
                //if (hasDraftInvoice)
                //{
                //    throw new BigBangException("فاکتوری در وضعیت ثبت موقت وجود دارد. برای انتقال به این مرحله، ابتدا باید آن را تایید نهایی کنید.");
                //}
            }

            int maxNoInNextLevel = _session.QueryOver<SALSOPDocument>()
                                           .Where(d => d.DocumentType.ID == nextTypeId)
                                           .Select(Projections.Max<SALSOPDocument>(d => d.No))
                                           .SingleOrDefault<int>();

            int newDocumentNo = maxNoInNextLevel == 0 ? 1 : maxNoInNextLevel + 1;

            SALSOPDocument newDoc = CloneDocument(currentDocument, nextTypeId, newDocumentNo);
            _session.Save(newDoc);

            _changeStateHistoryService.Add((SOPChangeStateHistoryEntityType)nextDocumentType.Code,
                                           newDoc.ID,
                                           (SOPChangeStateHistoryEntityStateType)newDoc.StateID,
                                           "");

            if (currentDocument.SOPDocumentItems != null)
            {
                var itemIds = currentDocument.SOPDocumentItems.Select(x => x.ID).ToList();

                var sourceAdjustments = _session.Query<SALSOPDocumentAdjustment>()
                                                .Where(a => itemIds.Contains(a.DocumentItem.ID))
                                                .ToList();

                foreach (SALSOPDocumentItem item in currentDocument.SOPDocumentItems)
                {
                    SALSOPDocumentItem newItem = CloneItem(item, newDoc);
                    _session.Save(newItem);

                    foreach (var srcAdjustment in sourceAdjustments.Where(a => a.DocumentItem.ID == item.ID && !a.Adjustment.IsDefault))/*پیش فرض ها در ادینگ داکیمنت آیتم ثبت می شوند*/
                    {
                        SALSOPDocumentAdjustment newAdjustment = CloneAdjustment(srcAdjustment, newItem);
                        _session.Save(newAdjustment);
                    }
                }
            }

        }

        /// <summary>
        /// زمانی اتفاق میافتد که کاستوم کامند تعدیل زده بشود
        /// </summary>
        /// <param name="document"></param>
        /// <param name="adjustment"></param>
        public virtual void CalculateAdjustment(SALSOPDocument document, SALSOPAdjustment adjustment)
        {
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToInvoice", "SalesRepresentativ", true);

            ValidateDocument(document);

            ISession session = _session ?? ServiceLocator.Get<ISession>();
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessDocument", "SalesFinance", true);
            List<SALSOPDocumentItem> documentItems = session.Query<SALSOPDocumentItem>()
                                                            .Where(x => x.Document.ID == document.ID)
                                                            .ToList();

            if (documentItems.Count == 0)
                return;

            var adjustmentName = session.Query<SALSOPAdjustment>()
                .Where(x => x.ID == adjustment.ID)
                .Select(x => x.AdjustmentType.Name)
                .FirstOrDefault();

            DateTime today = DateTime.Now.Date;
            DateTime tomorrow = today.AddDays(1);

            var validItemIds = documentItems.Where(x => x.ItemID != null).Select(x => x.ItemID.Id).Distinct().ToList();

            List<int> exemptItemIds = new List<int>();
            if (validItemIds.Any())
            {
                exemptItemIds = session.Query<SALSOPProductDetailTaxExemption>()
                    .Where(w => validItemIds.Contains(w.ProductDetailId.Id)
                             && w.FromDate < tomorrow
                             && (w.ToDate == null || w.ToDate >= today))
                    .Select(w => w.ProductDetailId.Id)
                    .ToList();
            }

            var adjustmentDocuments = new List<SALSOPDocumentAdjustment>();

            foreach (var item in documentItems)
            {
                decimal rawAdjustmentAmount = (decimal)((adjustment.Amount == 0m)
                    ? ((item.UnitPrice * adjustment.Rate) / 100) * item.Quantity
                    : adjustment.Amount * ((item.UnitPrice * item.Quantity) / item.Document.Amount));

                decimal adjustmentAmount = RoundToScale(rawAdjustmentAmount);

                bool isTaxExempt = item.ItemID != null && exemptItemIds.Contains(item.ItemID.Id);

                if (adjustmentName == Tax)
                {
                    if (!isTaxExempt)
                    {
                        var adjustmentDocument = new SALSOPDocumentAdjustment
                        {
                            Adjustment = adjustment,
                            DocumentItem = item,
                            AdjustmentAmount = adjustmentAmount,
                            EntityState = Metadata.Models.EntityState.Added
                        };
                        adjustmentDocuments.Add(adjustmentDocument);

                        item.Tax += adjustmentAmount;
                    }
                }
                else
                {
                    var adjustmentDocument = new SALSOPDocumentAdjustment
                    {
                        Adjustment = adjustment,
                        DocumentItem = item,
                        AdjustmentAmount = adjustmentAmount,
                        EntityState = Metadata.Models.EntityState.Added
                    };
                    adjustmentDocuments.Add(adjustmentDocument);

                    switch (adjustmentName)
                    {
                        case Discount:
                            item.Discount += adjustmentAmount;
                            break;
                        case AdditionalCharges:
                            item.AdditionalCharge += adjustmentAmount;
                            break;
                    }
                }

                item.TotalPrice = RoundToScale((item.UnitPrice * item.Quantity)
                                              - item.Discount
                                              + item.Tax
                                              + item.AdditionalCharge);

                item.EntityState = Metadata.Models.EntityState.Modified;
            }

            if (adjustmentDocuments.Any())
            {
                ServiceLocator.Get<IBulkRepository<SALSOPDocumentAdjustment>>().Save(adjustmentDocuments);
            }

            ServiceLocator.Get<IBulkRepository<SALSOPDocumentItem>>()
                .Builder
                .Entities(documentItems)
                .AllowedProperties(
                    d => d.Discount,
                    d => d.Tax,
                    d => d.AdditionalCharge,
                    d => d.TotalPrice,
                    d => d.EntityState
                )
                .Save();

            decimal subTotalAgg = RoundToScale(documentItems.Sum(x => x.Quantity * x.UnitPrice));
            decimal discountAgg = RoundToScale(documentItems.Sum(x => x.Discount));
            decimal taxAgg = RoundToScale(documentItems.Sum(x => x.Tax));
            decimal additionalAgg = RoundToScale(documentItems.Sum(x => x.AdditionalCharge));
            decimal net = RoundToScale(subTotalAgg - discountAgg + taxAgg + additionalAgg);

            document.DiscountAmount = discountAgg;
            document.TaxAmount = taxAgg;
            document.AdditionalChargesAmount = additionalAgg;
            document.TotalAmount = net;
            document.EntityState = Metadata.Models.EntityState.Modified;

            session.SaveOrUpdate(document);
        }

        private static SALSOPDocument CloneDocument(SALSOPDocument src, long nextTypeId, int newDocumentNo) => new()
        {
            CustomerID = src.CustomerID,
            IssueDate = src.IssueDate,
            Seller = src.Seller,
            Description = src.Description,
            AdditionalChargesAmount = src.AdditionalChargesAmount,
            DiscountAmount = src.DiscountAmount,
            TaxAmount = src.TaxAmount,
            TotalAmount = src.TotalAmount,
            No = newDocumentNo,
            Amount = src.Amount,
            EndDate = src.EndDate,
            StartDate = src.StartDate,
            Document = src,
            DocumentType = new SALSOPDocumentType { ID = (int)nextTypeId },
            StateID = SOPChangeStateHistoryEntityStateType.Draft,

        };

        private static SALSOPDocumentItem CloneItem(SALSOPDocumentItem src, SALSOPDocument parent) => new()
        {
            Document = parent,
            ProductAndServiceRate = src.ProductAndServiceRate,
            Quantity = src.Quantity,
            UnitPrice = src.UnitPrice,
            Discount = src.Discount,
            AdditionalCharge = src.AdditionalCharge,
            Amount = src.Amount,
            Description = src.Description,
            ItemID = src.ItemID,
            PrintDescription = src.PrintDescription,
            Tax = src.Tax,
            TotalPrice = src.TotalPrice,

        };

        private static SALSOPDocumentAdjustment CloneAdjustment(SALSOPDocumentAdjustment src,
                                                                SALSOPDocumentItem parent) => new()
                                                                {
                                                                    DocumentItem = parent,
                                                                    Adjustment = src.Adjustment,
                                                                    AdjustmentAmount = src.AdjustmentAmount,
                                                                };

        private void ValidateDocument(SALSOPDocument document)
        {
            var res = _documentService.ValidateConfirmDiscardedStatus(document);/*فقط در وضعیت های ویرایش و حذف بایداز خروجی آن استفاده کرد */
            if (!res.Item1)
            {
                throw new BigBangException(res.Item2);
            }
        }

    }
}