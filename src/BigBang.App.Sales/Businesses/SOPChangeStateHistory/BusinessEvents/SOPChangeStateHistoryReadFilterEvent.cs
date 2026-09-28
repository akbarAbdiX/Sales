using System;
using System.Collections.Generic;
using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.SOP.Concrete;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Services;
using NHibernate;
using NHibernate.Criterion;

namespace BigBang.App.Sales.Businesses.SOPDocument.BusinessEvents
{


    [EventSubscription(typeof(IBusinessReadFilterEvent<SALSOPChangeStateHistory>))]
    public class SALSOPChangeStateHistoryReadFilterEvent : IBusinessReadFilterEvent<SALSOPChangeStateHistory>
    {
        private readonly IBusinessContext _businessContext;
        private readonly ISession _session;
        private readonly IBigBangIdentityService _bigBangIdentityService;
        private readonly IChangeStateHistoryService _changeStateHistoryService = null;

        public SALSOPChangeStateHistoryReadFilterEvent(IBusinessContext businessContext,
                                                       ISession session,
                                                       IBigBangIdentityService bigBangIdentityService,
                                                       IChangeStateHistoryService changeStateHistoryService)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _bigBangIdentityService = bigBangIdentityService ?? throw new ArgumentNullException(nameof(bigBangIdentityService));
            _businessContext = businessContext ?? throw new ArgumentNullException(nameof(businessContext));
            _changeStateHistoryService = changeStateHistoryService ?? throw new ArgumentNullException(nameof(changeStateHistoryService));
        }

        public void OnRead(IBusinessReadFilter<SALSOPChangeStateHistory> filter)
        {
            var query = filter.QueryOverOf();
            query.Where(x => x.IsCurrent == true);
        }
    }
}
