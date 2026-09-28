using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.Role;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Sales.Businesses.SALDocumentAdjustment.BusinessEvents
{
    [EventSubscription(typeof(IBusinessDeletingEvent<SALSOPDocumentAdjustment>))]
    public class SOPDocumentAdjustmentDeletingEvent : IBusinessDeletingEvent<SALSOPDocumentAdjustment>
    {
        private readonly IDocumentAdjustmentService _DocumentAdjustmentService;
        private readonly ICheckAccess _checkAccess;

        public SOPDocumentAdjustmentDeletingEvent(IDocumentAdjustmentService documentAdjustmentService, ICheckAccess checkAccess)
        {
            _DocumentAdjustmentService = documentAdjustmentService;
            _checkAccess = checkAccess;
        }

        public void OnDeleting(IBusinessDeletingEventContext<SALSOPDocumentAdjustment> context)
        {
            _DocumentAdjustmentService.DocumentAdjustmentOnDeleting(context.Entity);
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessDocument", "SalesFinance", true);
        }
    }
}
