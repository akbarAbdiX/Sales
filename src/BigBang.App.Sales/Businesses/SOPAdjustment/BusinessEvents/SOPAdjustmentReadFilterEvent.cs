using System;
using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using NHibernate;

namespace BigBang.App.Sales.Businesses.SOPAdjustment.BusinessEvents
{
    [EventSubscription(typeof(IBusinessReadFilterEvent<SALSOPAdjustment>))]
    public class SOPAdjustmentBusinessReadFilterEvent : IBusinessReadFilterEvent<SALSOPAdjustment>
    {
        private readonly IBusinessContext _businessContext;
        private readonly ISession _session;

        public SOPAdjustmentBusinessReadFilterEvent()
        {
        }

        public SOPAdjustmentBusinessReadFilterEvent(IBusinessContext businessContext, ISession session)
        {
            _businessContext = businessContext;
            _session = session;
        }

        public void OnRead(IBusinessReadFilter<SALSOPAdjustment> filter)
        {
            if (_businessContext.HasKey("FilterAdjustment"))
            {
                var documentItemId = Convert.ToInt32(_businessContext["FilterAdjustment"]);
                var documentItem = _session.Query<SALSOPDocumentItem>().FirstOrDefault(d => d.ID == documentItemId);

                var document = documentItem?.Document;
                if (document == null)
                    return;


                var date = document.StartDate.HasValue
                    ? document.StartDate
                    : (document.IssueDate != default(DateTime) ? (DateTime?)document.IssueDate : null);

                if (!date.HasValue)
                    return;

                var effectiveDate = date.Value;

                filter.QueryOverOf().Where(a =>
                    (a.FromDate == null || a.FromDate.Value <= effectiveDate) &&
                    (a.ToDate == null || a.ToDate.Value >= effectiveDate));

            }
            //TODO
            if (_businessContext.HasKey("FilterCalculateAdjustment"))
            {
                var documentId = Convert.ToInt32(_businessContext["FilterCalculateAdjustment"]);
                var document = _session.Query<SALSOPDocument>().FirstOrDefault(d => d.ID == documentId);


                if (document == null)
                    return;


                var date = document.StartDate.HasValue
                    ? document.StartDate
                    : (document.IssueDate != default(DateTime) ? (DateTime?)document.IssueDate : null);

                if (!date.HasValue)
                    return;

                var effectiveDate = date.Value;

                filter.QueryOverOf().Where(a =>
                    (a.FromDate == null || a.FromDate.Value <= effectiveDate) &&
                    (a.ToDate == null || a.ToDate.Value >= effectiveDate));

            }
            return;

        }
    }
}