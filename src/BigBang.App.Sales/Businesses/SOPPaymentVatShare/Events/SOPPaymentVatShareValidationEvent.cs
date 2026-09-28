using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;

namespace BigBang.App.Sales.Businesses.SOPPaymentVatShare.Events;

[EventSubscription(typeof(IValidationEvent<SALSOPPaymentVatShare>))]
public class SOPPaymentVatShareValidationEvent : IValidationEvent<SALSOPPaymentVatShare>
{

    public SOPPaymentVatShareValidationEvent()
    {
    }
    #region Implementation of IValidationEvent<in SALSOPPaymentVatShare>
    public void OnValidate(IValidationContext<SALSOPPaymentVatShare> context)
    {
        if (context.Entity.ToDate.HasValue
            && context.Entity.ToDate.Value.Date < context.Entity.FromDate.Date)
        {
            throw new BigBangException("از تاریخ نمی تواند بزرگتر از تا تاریخ باشد.");
        }
    }
    #endregion
}