using System;
using System.Diagnostics.Contracts;
using System.Linq;
using BigBang.App.Sales.Businesses.SOPProductAndServiceRate;
using BigBang.App.Sales.Common.Constants;
using BigBang.App.Sales.Common.Exceptions;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;
using BigBang.App.Sales.Service.Role;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;

namespace BigBang.App.Sales.Businesses.SALDocument
{
    [EventSubscription(typeof(IValidationEvent<SALSOPDocument>))]
    public class SOPDocumentValidationEvent : IValidationEvent<SALSOPDocument>
    {
        private readonly IDocumentService _documentService;
        private readonly ICheckAccess _checkAccess;
        private readonly ISession _session;

        public SOPDocumentValidationEvent()
        {

        }
        public SOPDocumentValidationEvent(IDocumentService documentService, ICheckAccess checkAccess, ISession session)
        {
            _checkAccess = checkAccess;
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
            _session = session;
        }

        public void OnValidate(IValidationContext<SALSOPDocument> context)
        {
            var document = _documentService.GetByID(context.Entity.ID);
            var res = _documentService.ValidateConfirmDiscardedStatus(context.Entity, document != null ? document.StateID : null);/*فقط در وضعیت های ویرایش و حذف بایداز خروجی آن استفاده کرد */
            /*کارشناس فروش حق انجام هیچ اقدامی را بر روی این فرم ندارد */
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToInvoice", "SalesRepresentativ", true);

            switch (context.ValidationAction)
            {
                case ValidationAction.Add:
                    ValidateDateRange(context.Entity);
                    _documentService.DocumentValidation(context.Entity);
                    _documentService.DocumentstateMachineValidationForAdding(context.Entity);
                    _documentService.ValidateContractNumber(context.Entity);
                    _documentService.ValidateUniqueTaxIdentifier(context.Entity);
                    ValidateItemRatesOnDocumentDate(context.Entity);
                    _documentService.DocumentValidation(context.Entity);
                    _documentService.DocumentstateMachineValidationForAdding(context.Entity);
                    _documentService.ValidateContractNumber(context.Entity);
                    break;
                case ValidationAction.Update:

                    ValidateDateRange(context.Entity);

                    bool resmovingToForward = _documentService.ValidateEntityStatusToMovingToForward(context.Entity, document != null ? document.StateID : null);
                    if (!resmovingToForward)
                    {
                        /*
                         * تغییر وضعیت صورت نگرفته است و صرفا داده های فرم تغییر کرده است`
                         * لذا باید شرایط دسترسی های کاربران را هم چک کرد
                         */
                        _checkAccess.CheckDocumentsAccess<bool>("CheckAccessDocument", "SalesFinance", true);
                    }

                    _documentService.ValidateContractNumber(context.Entity);
                    _documentService.ValidateUniqueTaxIdentifier(context.Entity);

                    res = _documentService.ValidateConfirmDiscardedStatus(context.Entity, document != null ? document.StateID : null, "ویرایش");
                    if (!res.Item1)
                    {
                        throw new BigBangException(res.Item2);
                    }
                    ValidateItemRatesOnDocumentDate(context.Entity);
                    _documentService.ValidateContractNumber(context.Entity);
                    break;
                case ValidationAction.Delete:

                    res = _documentService.ValidateConfirmDiscardedStatus(context.Entity, document != null ? document.StateID : null, "حـذف");
                    if (!res.Item1)
                        throw new BigBangException(res.Item2);

                    ValidateDocumentRemoving(context.Entity);

                    break;
            }
        }

        private void ValidateItemRatesOnDocumentDate(SALSOPDocument document)
        {
            if (!ProductAndServiceRateValidity.IsInvoiceOrProforma(document) || document.ID == 0)
                return;

            var documentDate = ProductAndServiceRateValidity.GetDocumentDate(document);
            var items = _session.Query<SALSOPDocumentItem>()
                .Where(x => x.Document.ID == document.ID)
                .ToList();

            foreach (var item in items)
            {
                var rate = item.ProductAndServiceRate;
                if (rate == null || rate.ID == 0)
                    continue;

                if (!ProductAndServiceRateValidity.IsValidOn(rate, documentDate))
                    throw Exceptions.Raise(ExceptionCode.ProductAndServiceRateDateWasExpier);
            }
        }

        private static void ValidateDateRange(SALSOPDocument entity)
        {
            var documentTypeName = entity.DocumentType?.Name;

            if (documentTypeName == SaleConstant.ProformaInvoice)
                ValidateProformaDates(entity);
            else if (documentTypeName == SaleConstant.Contract)
                ValidateContractDates(entity);
        }

        private static void ValidateProformaDates(SALSOPDocument entity)
        {
            if (entity.EndDate.HasValue
                && entity.EndDate.Value.Date < entity.IssueDate.Date)
            {
                throw new BigBangException("تاریخ اعتبار نمی‌تواند کوچکتر از تاریخ صدور باشد.");
            }
        }

        private static void ValidateContractDates(SALSOPDocument entity)
        {
            if (entity.StartDate.HasValue
                && entity.StartDate.Value.Date < entity.IssueDate.Date)
            {
                throw new BigBangException("تاریخ شروع قرارداد نمی‌تواند کوچکتر از تاریخ صدور باشد.");
            }

            if (!entity.EndDate.HasValue)
                return;

            var endDate = entity.EndDate.Value.Date;

            if (entity.StartDate.HasValue && endDate < entity.StartDate.Value.Date)
            {
                throw new BigBangException("تاریخ پایان قرارداد نمی‌تواند کوچکتر از تاریخ شروع قرارداد باشد.");
            }

            if (endDate < entity.IssueDate.Date)
            {
                throw new BigBangException("تاریخ پایان قرارداد نمی‌تواند کوچکتر از تاریخ صدور باشد.");
            }
        }

        private static void ValidateDocumentRemoving(SALSOPDocument document)
        {
            if (document.SOPDocumentItems == null || (document.SOPDocumentItems != null && document.SOPDocumentItems.Count == 0))
                return;

            string documentTypeName = "";

            switch ((SOPChangeStateHistoryEntityType)document.DocumentType.Code)
            {
                case SOPChangeStateHistoryEntityType.ProformaInvoice:/*پیش فاکتور*/
                    documentTypeName = "پیش فاکتور";
                    break;

                case SOPChangeStateHistoryEntityType.Order:/*سفارش*/
                    documentTypeName = "سفارش";
                    break;

                case SOPChangeStateHistoryEntityType.Contract:/*قرارداد*/
                    documentTypeName = "قرارداد";
                    break;

                case SOPChangeStateHistoryEntityType.InvoiceIssuanceRequest:/*درخواست صدور فاکتور*/
                    documentTypeName = "درخواست صدور فاکتور";
                    break;

                case SOPChangeStateHistoryEntityType.Invoice:/*فاکتور*/
                    documentTypeName = "فاکتور";
                    break;

            }

            throw new BigBangException(string.Format("ابتدا اقلام {0} حذف شود، سپس اقدام به حذف {0} صورت گیرد.", documentTypeName));

        }

    }
}