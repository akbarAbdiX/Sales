using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;

namespace BigBang.App.Sales.Businesses.SOPContractStage.Events;

[EventSubscription(typeof(IValidationEvent<SALSOPContractStage>))]
public class SOPContractStageValidationEvent : IValidationEvent<SALSOPContractStage>
{
    public void OnValidate(IValidationContext<SALSOPContractStage> context)
    {
        switch (context.ValidationAction)
        {
            case ValidationAction.Add:
            case ValidationAction.Update:
                ValidateDateRange(context.Entity);
                break;
        }
    }

    private static void ValidateDateRange(SALSOPContractStage entity)
    {
        if (entity.EndDate.HasValue
            && entity.EndDate.Value.Date < entity.StartDate.Date)
        {
            throw new BigBangException("تاریخ پایان نمی‌تواند کوچکتر از تاریخ شروع باشد.");
        }
    }
}