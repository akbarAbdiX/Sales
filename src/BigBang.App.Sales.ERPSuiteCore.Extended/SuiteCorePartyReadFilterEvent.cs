using BigBang.App.ERPSuiteCore.Domain;
using BigBang.App.ERPSuiteCore.Domain.Enums;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using NHibernate;

namespace BigBang.App.Sales.ERPSuiteCore.Extended
{
    [EventSubscription(typeof(IBusinessReadFilterEvent<SuiteCoreParty>))]
    public class SuiteCorePartyReadFilterEvent : IBusinessReadFilterEvent<SuiteCoreParty>
    {
        private readonly ISession _session;
        private readonly IBusinessContext _businessContext;
        public SuiteCorePartyReadFilterEvent(ISession session, IBusinessContext businessContext)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _businessContext = businessContext;
        }

        public void OnRead(IBusinessReadFilter<SuiteCoreParty> filter)
        {
            if (_businessContext.HasKey("FilterParty"))
            {
                var contactSubquery = _session.Query<SuiteCoreContact>().Where(c => c.State == CustomerState.Active).Select(c => c.Id).ToList();
                var employeeSubquery = _session.Query<SuiteCoreEmployee>().Where(c => c.State == CustomerState.Active).Select(e => e.Id).ToList();
                var supplierSubquery = _session.Query<SuiteCoreIndividualSupplier>().Where(c => c.State == CustomerState.Active).Select(s => s.Id).ToList();
                var res = contactSubquery.Union(employeeSubquery).Union(supplierSubquery).ToList();
                filter.Items = filter.QueryOverOf().WhereRestrictionOn(t => t.Id).IsIn(res);
            }
        }
    }
}
