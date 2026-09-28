using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.Role;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Sales.Businesses.SOPDocumentAdjustment.BusinessEvents
{
    [EventSubscription(typeof(IBusinessUpdatedEvent<SALSOPDocumentAdjustment>))]
    public class SOPDocumentAdjustmentUpdatedEvent : IBusinessUpdatedEvent<SALSOPDocumentAdjustment>
    {
        private readonly IDocumentAdjustmentService _DocumentAdjustmentService;
        private readonly ICheckAccess _checkAccess;

        public SOPDocumentAdjustmentUpdatedEvent(ICheckAccess checkAccess, IDocumentAdjustmentService documentAdjustmentService)
        {
            _checkAccess = checkAccess;
            _DocumentAdjustmentService = documentAdjustmentService;
        }

        public void OnUpdated(IBusinessUpdatedEventContext<SALSOPDocumentAdjustment> context)
        {
            _DocumentAdjustmentService.DocumentAdjustmentOnUpdating(context.Entity);
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessDocument", "SalesFinance", true);
        }
    }
}
