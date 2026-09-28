using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;

namespace BigBang.App.Accounting.Business.GLCDocument.BusinessEvents
{
    [EventSubscription(typeof(IBusinessLoadedEvent<SALSOPDocumentItem>))]
    public class SOPDocumentItemBusinessLoadedEvent : IBusinessLoadedEvent<SALSOPDocumentItem>
    {
        private readonly IBusinessContext _businessContext;
        public SOPDocumentItemBusinessLoadedEvent(IBusinessContext businessContext)
        {
            _businessContext = businessContext;
        }
        public void OnLoaded(IBusinessLoadedEventContext<SALSOPDocumentItem> context)
        {
            if (_businessContext.HasKey("LoadProductServiceCode"))
            {
                foreach (var item in context.Items)
                {
                    item.ProductServiceCode = item.ItemID.ProductServiceCode;
                }
            }
        }
    }
}