using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Sales.Businesses.SOPAdjustment.BusinessEvents
{
    [EventSubscription(typeof(IBusinessUpdatingEvent<SALSOPAdjustment>))]
    public class SOPAdjustmentUpdatingEvent : IBusinessUpdatingEvent<SALSOPAdjustment>
    {
        private readonly IAdjustmentService _adjustmentService;
        public SOPAdjustmentUpdatingEvent(IAdjustmentService adjustmentService) => _adjustmentService = adjustmentService;
        public void OnUpdating(IBusinessUpdatingEventContext<SALSOPAdjustment> context)
        {
            _adjustmentService.AdjustmentOnUpdating(context.Entity);
        }
    }
}