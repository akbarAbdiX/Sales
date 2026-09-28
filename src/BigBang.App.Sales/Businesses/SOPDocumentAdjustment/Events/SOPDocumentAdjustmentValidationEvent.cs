using System;
using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.Role;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;

namespace BigBang.App.Sales.Businesses.SOPDocumentAdjustment.Events;

[EventSubscription(typeof(IValidationEvent<SALSOPDocumentAdjustment>))]
public class SOPDocumentAdjustmentValidationEvent : IValidationEvent<SALSOPDocumentAdjustment>
{
    private readonly ICheckAccess _checkAccess;
    private readonly IDocumentService _documentService;
    private readonly ISession _session;

    public SOPDocumentAdjustmentValidationEvent(ICheckAccess checkAccess,
                                                IDocumentService documentService,
                                                ISession session)
    {
        _checkAccess = checkAccess ?? throw new ArgumentNullException(nameof(checkAccess));
        _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    #region [ Implementation of IValidationEvent<in SALSOPDocumentAdjustment> ]

    public void OnValidate(IValidationContext<SALSOPDocumentAdjustment> context)
    {
        var res = _documentService.ValidateConfirmDiscardedStatus(context.Entity.DocumentItem.Document, null, "تغییرات، حذف و اضافه آیتم های فرم");/*فقط در وضعیت های ویرایش و حذف بایداز خروجی آن استفاده کرد */
        if (!res.Item1)
            throw new BigBangException(res.Item2);

        _checkAccess.CheckDocumentsAccess<bool>("CheckAccessToInvoiceItems", "SalesRepresentativ", true);

        if (context.ValidationAction == ValidationAction.Add || context.ValidationAction == ValidationAction.Update)
        {
            ValidateSingleAdjustmentPerType(context);
            ValidateAdjustmentDateRange(context);
            ValidateDocumentDateWithinAdjustmentRange(context);
        }
    }

    private void ValidateSingleAdjustmentPerType(IValidationContext<SALSOPDocumentAdjustment> context)
    {
        if (context.Entity.DocumentItem.Document == null || context.Entity.DocumentItem == null || context.Entity.Adjustment == null)
            return;

        var adjustment = _session.Get<SALSOPAdjustment>(context.Entity.Adjustment.ID);
        var adjustmentType = adjustment?.AdjustmentType;

        if (adjustmentType == null)
            return;

        string typeTitle = GetAdjustmentTypeTitle(adjustmentType);

        var document = _documentService.GetByID(context.Entity.DocumentItem.Document.ID);
        if (document == null /*در حال ایجاد آیتم می باشد*/)
            return;

        var docItem = document.SOPDocumentItems.FirstOrDefault(x => x.ID == context.Entity.DocumentItem.ID);
        if (docItem == null)
            return;

        int existingCount = docItem.SOPDocumentAdjustments.Count(x => x.Adjustment.AdjustmentType == context.Entity.Adjustment.AdjustmentType);

        if (context.ValidationAction == ValidationAction.Add && existingCount > 0)
        {
            /*It means that at least there is one item with this definition, So raise error! */
            throw new BigBangException($"برای این آیتم سند قبلاً یک تعدیل از نوع «{typeTitle}» ثبت شده است؛ از هر نوع تعدیل (تخفیف، هزینه اضافی، مالیات) تنها یک مورد مجاز است.");
        }

        if (context.ValidationAction == ValidationAction.Update && existingCount == 1)
        {
            /*It means the detected item is itself! so its ok */
            var adjustItem = docItem.SOPDocumentAdjustments.FirstOrDefault(x => x.Adjustment.AdjustmentType == context.Entity.Adjustment.AdjustmentType);
            if (adjustItem != null && adjustItem.ID != context.Entity.ID)
                throw new BigBangException($"برای این آیتم سند قبلاً یک تعدیل از نوع «{typeTitle}» ثبت شده است؛ از هر نوع تعدیل (تخفیف، هزینه اضافی، مالیات) تنها یک مورد مجاز است.");
        }
    }

    /// <summary>
    /// تاریخ «تا»ی تعدیل نمی‌تواند قبل از تاریخ «از» آن باشد.
    /// </summary>
    private void ValidateAdjustmentDateRange(IValidationContext<SALSOPDocumentAdjustment> context)
    {
        var adjustmentRef = context.Entity.Adjustment;
        if (adjustmentRef == null)
            return;

        var adjustment = _session.Get<SALSOPAdjustment>(adjustmentRef.ID);
        if (adjustment == null)
            return;

        if (adjustment.ToDate.HasValue && adjustment.FromDate.HasValue && adjustment.ToDate.Value < adjustment.FromDate.Value)
        {
            string typeTitle = GetAdjustmentTypeTitle(adjustment.AdjustmentType);
            throw new BigBangException($"در تعدیل «{typeTitle}» تا تاریخ نمی‌تواند قبل از تاریخ  باشد.");
        }
    }

    /// <summary>
    /// تعدیل فقط در صورتی قابل اعمال است که تاریخ سند در بازه زمانی (از تاریخ - تا تاریخ) تعدیل قرار داشته باشد.
    /// در غیر این صورت از ثبت/ویرایش این تعدیل برای سند جلوگیری می‌شود.
    /// </summary>
    private void ValidateDocumentDateWithinAdjustmentRange(IValidationContext<SALSOPDocumentAdjustment> context)
    {
        if (context.Entity.DocumentItem?.Document == null || context.Entity.Adjustment == null)
            return;

        var adjustment = _session.Get<SALSOPAdjustment>(context.Entity.Adjustment.ID);
        if (adjustment == null)
            return;

        DateTime? documentDate = context.Entity.DocumentItem.Document.StartDate;
        string typeTitle = GetAdjustmentTypeTitle(adjustment.AdjustmentType);

        if (!documentDate.HasValue)
            return; /* بدون تاریخ سند نمی‌توان بازه را اعتبارسنجی کرد */

        if (adjustment.FromDate.HasValue && documentDate.Value < adjustment.FromDate.Value)
        {
            throw new BigBangException($"تاریخ سند زودتر از «از تاریخ» بازه‌ی تعدیل «{typeTitle}» است و این تعدیل قابل اعمال نیست.");
        }

        if (adjustment.ToDate.HasValue && documentDate.Value > adjustment.ToDate.Value)
        {
            throw new BigBangException($"تاریخ سند از بازه‌ی اعتبار تعدیل «{typeTitle}» گذشته است، بنابراین این تعدیل قابل اعمال نیست.");
        }
    }

    private static string GetAdjustmentTypeTitle(dynamic adjustmentType)
    {
        if (adjustmentType == null)
            return string.Empty;

        return !string.IsNullOrWhiteSpace(adjustmentType.Title) ? adjustmentType.Title : adjustmentType.Name;
    }

    #endregion

}