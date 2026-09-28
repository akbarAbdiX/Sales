using System;
using System.Linq;
using AngleSharp.Dom;
using BigBang.App.ERPSuiteCore.Domain;
using BigBang.App.Sales.Businesses.SOPProductAndServiceRate;
using BigBang.App.Sales.Common.Exceptions;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.Role;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.Metadata.Models;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;
using NHibernate.Criterion;
using Polly;

namespace BigBang.App.Sales.Businesses.SOPDocumentItem.Events;

[EventSubscription(typeof(IValidationEvent<SALSOPDocumentItem>))]
public class SOPDocumentItemValidationEvent : IValidationEvent<SALSOPDocumentItem>
{
    private readonly ICheckAccess _checkAccess;
    private readonly IDocumentService _documentService;
    private readonly ISession _session;

    public SOPDocumentItemValidationEvent(ICheckAccess checkAccess,
                                          IDocumentService documentService,
                                          ISession session)
    {
        _checkAccess = checkAccess;
        _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    #region Implementation of IValidationEvent<in SALSOPDocumentItem>
    public void OnValidate(IValidationContext<SALSOPDocumentItem> context)
    {
        var res = _documentService.ValidateConfirmDiscardedStatus(context.Entity.Document, null, "تغییرات، حذف و اضافه آیتم های فرم");/*فقط در وضعیت های ویرایش و حذف بایداز خروجی آن استفاده کرد */
        if (!res.Item1)
            throw new BigBangException(res.Item2);

        _checkAccess.CheckDocumentsAccess<bool>("CheckAccessDocumentItem", "SalesFinance", true);
        _checkAccess.CheckDocumentsAccess<bool>("CheckAccessDocumentAdjustment", "SalesFinance", true);
        _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToInvoiceItems", "SalesRepresentativ", true);

        if (context.ValidationAction == ValidationAction.Delete)
            return;

        ValidateFields(context.Entity);

        if (context.ValidationAction == ValidationAction.Add || context.ValidationAction == ValidationAction.Update)
        {
            ValidateSingleItemPerType(context);
        }
    }

    private void ValidateFields(SALSOPDocumentItem entity)
    {
        if (entity.Quantity == 0)
            throw new BigBangException("مقدار نمی‌تواند صفر باشد.");
        if (entity.Amount < 0)
            throw new BigBangException("مبلغ نمی‌تواند منفی باشد.");
        if (entity.TotalPrice < 0)
            throw new BigBangException("مبلغ کل نمی‌تواند منفی باشد.");
        if (entity.Amount == 0)
            throw new BigBangException($"برای {entity.ItemID.ProductDetailName} نرخی تعیین نشده است. ");

        ValidateRateDateRange(entity);
    }

    private static void ValidateRateDateRange(SALSOPDocumentItem entity)
    {
        if (!ProductAndServiceRateValidity.IsInvoiceOrProforma(entity.Document))
            return;

        var rate = entity.ProductAndServiceRate;
        if (rate == null || rate.ID == 0)
            return;

        var documentDate = ProductAndServiceRateValidity.GetDocumentDate(entity.Document);
        if (!ProductAndServiceRateValidity.IsValidOn(rate, documentDate))
            throw Exceptions.Raise(ExceptionCode.ProductAndServiceRateDateWasExpier);
    }

    private void ValidateSingleItemPerType(IValidationContext<SALSOPDocumentItem> context)
    {
        if (context.Entity.Document == null)
            return;

        var document = _documentService.GetByID(context.Entity.Document.ID);
        if (document == null /*در حال ایجاد آیتم می باشد*/)
            return;

        int existingCount = document.SOPDocumentItems.Count(x => x.ItemID.Id == context.Entity.ItemID.Id);

        if (context.ValidationAction == ValidationAction.Add && existingCount > 0)
        {
            /*It means that at least there is one item with this definition, So raise error! */
            throw new BigBangException($"برای سند جاری نمی توان بیش از یک آیتم « {context.Entity.ItemID.ProductDetailName} » تعریف کرد.");
        }

        if (context.ValidationAction == ValidationAction.Update && existingCount == 1)
        {
            /*It means the detected item is itself! so its ok */

            var item = document.SOPDocumentItems.FirstOrDefault(x => x.ItemID.Id == context.Entity.ItemID.Id);
            if (item != null && item.ID != context.Entity.ID)
                throw new BigBangException($"برای سند جاری نمی توان بیش از یک آیتم « {context.Entity.ItemID.ProductDetailName} » تعریف کرد.");
        }
    }

    #endregion
}