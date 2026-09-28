using System;
using System.Linq;
using BigBang.App.ERPSuiteCore.Domain;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Services;
using NHibernate;

namespace BigBang.App.Sales.Businesses.SOPDocument.BusinessEvents
{


    [EventSubscription(typeof(IBusinessReadFilterEvent<SuiteCoreAddress>))]
    public class SuiteCoreAddressReadFilterEvent : IBusinessReadFilterEvent<SuiteCoreAddress>
    {
        private readonly IBusinessContext _businessContext;
        private readonly ISession _session;
        private readonly IBigBangIdentityService _bigBangIdentityService;
        private readonly IChangeStateHistoryService _changeStateHistoryService = null;

        public SuiteCoreAddressReadFilterEvent(IBusinessContext businessContext,
                                                       ISession session,
                                                       IBigBangIdentityService bigBangIdentityService,
                                                       IChangeStateHistoryService changeStateHistoryService)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _bigBangIdentityService = bigBangIdentityService ?? throw new ArgumentNullException(nameof(bigBangIdentityService));
            _businessContext = businessContext ?? throw new ArgumentNullException(nameof(businessContext));
        }

        public void OnRead(IBusinessReadFilter<SuiteCoreAddress> filter)
        {
            if (_businessContext.HasKey("FilterCustomerAddress"))
            {
                var customerId = Convert.ToInt32(_businessContext["FilterCustomerAddress"]);
                filter.QueryOverOf().Where(x => x.Party.Id == customerId).ToList();
            }

        }
    }
}
