using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.Role;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Sales.Businesses.SOPDocumentAdjustment.BusinessEvents
{
    [EventSubscription(typeof(IBusinessAddingEvent<SALSOPDocument>))]
    public class SOPDocumentAddingEvent : IBusinessAddingEvent<SALSOPDocument>
    {
        private readonly ICheckAccess _checkAccess;

        public SOPDocumentAddingEvent(ICheckAccess checkAccess,
            IBusinessContext businessContext)
        {
            _checkAccess = checkAccess;
        }

        public void OnAdding(IBusinessAddingEventContext<SALSOPDocument> context)
        {
            _checkAccess.CheckDocumentsAccess<bool>("CheckAccessDocument", "SalesFinance", true);
        }
    }
}