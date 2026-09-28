using AngleSharp.Dom;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.SOP.Concrete;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Sales.Businesses.SOPDocumentItem.BusinessEvents
{
    [EventSubscription(typeof(IBusinessUpdatingEvent<SALSOPDocumentItem>))]
    public class SOPDocumentItemUpdatingEvent : IBusinessUpdatingEvent<SALSOPDocumentItem>
    {
        private readonly IDocumentItemService _documentItemService;
        public SOPDocumentItemUpdatingEvent(IDocumentItemService documentItemService) => _documentItemService = documentItemService;
        public void OnUpdating(IBusinessUpdatingEventContext<SALSOPDocumentItem> context)
        {
            _documentItemService.DocumentItemOnUpdating(context.Entity);
        }
    }

}