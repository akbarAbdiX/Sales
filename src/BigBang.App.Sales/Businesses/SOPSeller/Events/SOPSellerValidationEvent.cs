using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;

namespace BigBang.App.Sales.Businesses.SOPSeller.Events;

[EventSubscription(typeof(IValidationEvent<SALSOPSeller>))]
public class SOPSellerValidationEvent : IValidationEvent<SALSOPSeller>
{
    private readonly ISession _session;

    public SOPSellerValidationEvent(ISession session)
    {
        _session = session;
    }

    public void OnValidate(IValidationContext<SALSOPSeller> context)
    {
        switch (context.ValidationAction)
        {
            case ValidationAction.Add:
            case ValidationAction.Update:
                ValidateDefault(context.Entity);
                break;
            case ValidationAction.Delete:
                if (context.Entity.IsDefault)
                    throw new BigBangException("آدرس پیش‌فرض را نمی‌توان حذف کرد.");
                break;
        }
    }

    private void ValidateDefault(SALSOPSeller entity)
    {
        if (!entity.IsDefault) return;

        var existingDefault = _session.Query<SALSOPSeller>()
            .Any(s => s.IsDefault && s.Id != entity.Id);

        if (existingDefault)
            throw new BigBangException("یک آدرس پیش‌فرض از قبل وجود دارد.");
    }
}
