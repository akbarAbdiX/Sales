using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.Role;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Sales.Businesses.SOPDocumentAdjustment.BusinessEvents
{
    [EventSubscription(typeof(IBusinessAddingEvent<SALSOPDocumentAdjustment>))]
    public class SOPDocumentAdjustmentAddingEvent : IBusinessAddingEvent<SALSOPDocumentAdjustment>
    {
        private readonly IDocumentAdjustmentService _DocumentAdjustmentService;
        private readonly ICheckAccess _checkAccess;

        public SOPDocumentAdjustmentAddingEvent(IDocumentAdjustmentService documentAdjustmentService, ICheckAccess checkAccess)
        {
            _DocumentAdjustmentService = documentAdjustmentService;
            _checkAccess = checkAccess;
        }

        public void OnAdding(IBusinessAddingEventContext<SALSOPDocumentAdjustment> context)
        {
            _DocumentAdjustmentService.DocumentAdjustmentOnAdding(context.Entity);
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessDocument", "SalesFinance", true);
        }
    }
}
