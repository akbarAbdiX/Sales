using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;

namespace BigBang.App.Sales.Businesses.SOPProductType.Events;

[EventSubscription(typeof(IValidationEvent<SALSOPProductType>))]
public class SOPProductTypeValidationEvent : IValidationEvent<SALSOPProductType>
{
    private readonly ISession _session;

    public SOPProductTypeValidationEvent()
    {

    }

    public SOPProductTypeValidationEvent(ISession session)
    {
        _session = session;
    }
    #region Implementation of IValidationEvent<in SALSOPProductType>
    public void OnValidate(IValidationContext<SALSOPProductType> context)
    {
        var exists = _session.Query<SALSOPProductType>()
         .Any(x => x.Code == context.Entity.Code);

        if (exists)
        {
            throw new BigBangException($"کد {context.Entity.Code} تکراری است.");

        }
    }
    #endregion
}