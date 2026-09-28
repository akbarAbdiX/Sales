using System;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Sales.Businesses.SOPProductAndServiceRate.Events;

[EventSubscription(typeof(IBusinessReadFilterEvent<SALSOPProductAndServiceRate>))]
public class ProductAndServiceRateBusinessReadFilterEvent : IBusinessReadFilterEvent<SALSOPProductAndServiceRate>
{
    private readonly IBusinessContext _businessContext;
    public ProductAndServiceRateBusinessReadFilterEvent(IBusinessContext businessContext)
    {
        _businessContext = businessContext;
    }
    public void OnRead(IBusinessReadFilter<SALSOPProductAndServiceRate> context)
    {
        if (!_businessContext.HasKey("FilterProductAndServiceRate"))
            return;

        var itemId = Convert.ToInt32(_businessContext["FilterProductAndServiceRate"]);
        var dayStart = GetDocumentDate();
        var nextDay = dayStart.AddDays(1);

        context.QueryOverOf().Where(x =>
            x.ItemID.Id == itemId
            && x.StartDate < nextDay
            && (x.EndDate == null || x.EndDate >= dayStart)).ToList();
    }

    private DateTime GetDocumentDate()
    {
        if (_businessContext.HasKey("FilterProductAndServiceRateDocumentDate"))
        {
            var raw = _businessContext["FilterProductAndServiceRateDocumentDate"];
            if (raw != null && DateTime.TryParse(raw.ToString(), out var parsed) && parsed != default)
                return parsed.Date;
        }

        return DateTime.Now.Date;
    }
}