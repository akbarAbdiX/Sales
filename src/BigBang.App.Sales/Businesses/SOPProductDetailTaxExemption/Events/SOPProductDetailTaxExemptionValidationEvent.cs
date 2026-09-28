using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;

namespace BigBang.App.Sales.Businesses.SOPProductDetailTaxExemption.Events;

[EventSubscription(typeof(IValidationEvent<SALSOPProductDetailTaxExemption>))]
public class SOPProductDetailTaxExemptionValidationEvent : IValidationEvent<SALSOPProductDetailTaxExemption>
{
    public void OnValidate(IValidationContext<SALSOPProductDetailTaxExemption> context)
    {
        switch (context.ValidationAction)
        {
            case ValidationAction.Add:
            case ValidationAction.Update:
                ValidateDateRange(context.Entity);
                break;
        }
    }

    private static void ValidateDateRange(SALSOPProductDetailTaxExemption entity)
    {
        if (entity.ToDate.HasValue
            && entity.ToDate.Value.Date < entity.FromDate.Date)
        {
            throw new BigBangException("تا تاریخ نمی‌تواند کوچکتر از «از تاریخ» باشد.");
        }
    }
}