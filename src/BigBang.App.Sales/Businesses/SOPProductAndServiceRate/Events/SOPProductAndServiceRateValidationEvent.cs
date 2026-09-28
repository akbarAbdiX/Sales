using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;

namespace BigBang.App.Sales.Businesses.SOPProductAndServiceRate.Events;

[EventSubscription(typeof(IValidationEvent<SALSOPProductAndServiceRate>))]
public class SOPProductAndServiceRateValidationEvent : IValidationEvent<SALSOPProductAndServiceRate>
{
    public void OnValidate(IValidationContext<SALSOPProductAndServiceRate> context)
    {
        switch (context.ValidationAction)
        {
            case ValidationAction.Add:

            case ValidationAction.Update:
                ValidateDateRange(context.Entity);
                VlidateUsedProductAndServiceRate(context.Entity);
                break;
        }
    }

    private static void VlidateUsedProductAndServiceRate(SALSOPProductAndServiceRate entity)
    {
        ISession session = ServiceLocator.Get<ISession>();
        bool isUsed = session.Query<SALSOPDocumentItem>()
     .Any(x => x.ProductAndServiceRate.ID == entity.ID);
        if (isUsed)
        {
            throw new BigBangException("این محصول با نرخ تعیین‌شده در فرم‌های دیگر استفاده شده است.");
        }

    }
    private static void ValidateDateRange(SALSOPProductAndServiceRate entity)
    {
        if (entity.EndDate.HasValue
            && entity.EndDate.Value.Date < entity.StartDate.Date)
        {
            throw new BigBangException("تا تاریخ نمی‌تواند کوچکتر از «از تاریخ» باشد.");
        }
    }
}