using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Sales.Businesses.SOPProductAndServiceRate.Events
{
    [EventSubscription(typeof(IBusinessLoadedEvent<SALSOPProductAndServiceRate>))]
    public class SOPProductAndServiceRateBusinessLoadedEvent : IBusinessLoadedEvent<SALSOPProductAndServiceRate>
    {
        public void OnLoaded(IBusinessLoadedEventContext<SALSOPProductAndServiceRate> context)
        {
            foreach (var item in context.Items)
            {
                item.ProductServiceCode = item.ItemID.ProductServiceCode;
            }
        }
    }
}