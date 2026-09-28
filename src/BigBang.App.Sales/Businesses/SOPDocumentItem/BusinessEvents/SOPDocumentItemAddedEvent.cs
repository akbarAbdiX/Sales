using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Sales.Businesses.SOPDocumentItem.BusinessEvents
{
    [EventSubscription(typeof(IEntityAddedEvent<SALSOPDocumentItem>))]
    public class SOPDocumentItemAddedEvent : IEntityAddedEvent<SALSOPDocumentItem>
    {
        private readonly IDocumentItemService _documentItemService;
        public SOPDocumentItemAddedEvent(IDocumentItemService documentItemService) => _documentItemService = documentItemService;

        public void OnAdded(IEntityAddedEventContext<SALSOPDocumentItem> context)
        {            
            _documentItemService.DocumentItemOnAdding(context.Entity);
        }
    }
}
