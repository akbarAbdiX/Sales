using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Sales.Businesses.SOPDocumentItem.BusinessEvents
{
    [EventSubscription(typeof(IEntityDeletingEvent<SALSOPDocumentItem>))]
    public class SOPDocumentItemDeletingEvent : IEntityDeletingEvent<SALSOPDocumentItem>
    {
        private readonly IDocumentItemService _DocumentItemService;
        public SOPDocumentItemDeletingEvent(IDocumentItemService documentItemService) => _DocumentItemService = documentItemService;
        public void OnDeleting(IEntityDeletingEventContext<SALSOPDocumentItem> context)
        {
                _DocumentItemService.DocumentItemOnDeleting(context.Entity);
        }
    }
}