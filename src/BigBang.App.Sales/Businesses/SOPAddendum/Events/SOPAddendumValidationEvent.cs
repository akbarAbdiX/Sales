using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;

namespace BigBang.App.Sales.Businesses.SOPAddendum.Events;

[EventSubscription(typeof(IValidationEvent<SALSOPAddendum>))]
public class SOPAddendumValidationEvent : IValidationEvent<SALSOPAddendum>
{
    public void OnValidate(IValidationContext<SALSOPAddendum> context)
    {
        switch (context.ValidationAction)
        {
            case ValidationAction.Add:
            case ValidationAction.Update:
                ValidateDateRange(context.Entity);
                break;
        }
    }

    private static void ValidateDateRange(SALSOPAddendum entity)
    {
        if (entity.EndDate.HasValue
            && entity.EndDate.Value.Date < entity.AmendmentDate.Date)
        {
            throw new BigBangException("تاریخ پایان جدید نمی‌تواند کوچکتر از تاریخ الحاقیه باشد.");
        }
    }
}