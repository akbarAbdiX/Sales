using System.Linq;
using BigBang.App.Sales.Common.Constants;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using NHibernate;

namespace BigBang.App.Sales.Business.SOPDocument.BusinessEvents
{
    [EventSubscription(typeof(IBusinessCreateInstanceEvent<SALSOPDocument>))]
    public class SOPDocumentCreateInstanceEvent : IBusinessCreateInstanceEvent<SALSOPDocument>
    {
        private IBusinessContext _businessContext;
        private ISession _session;
        private readonly IDocumentService _documentService;
        public SOPDocumentCreateInstanceEvent(IBusinessContext businessContext
                                             , ISession session
                                             , IDocumentService documentService)
        {
            _documentService = documentService;
            _businessContext = businessContext;
            _session = session;
        }
        public void OnCreateInstance(IBusinessCreateInstanceEventContext<SALSOPDocument> context)
        {
            SALSOPSeller seller = _session.Query<SALSOPSeller>().FirstOrDefault(s => s.IsDefault == true);
            context.Entity.Seller = seller;
            context.Entity.StateID = Domain.Enums.SOPChangeStateHistoryEntityStateType.Draft;

            if (_businessContext.BusinessContext.ContainsKey("DocumentTypeProformaInvoice"))
            {
                var documentType = _session.Query<SALSOPDocumentType>()
                                                         .FirstOrDefault(x => x.Name == SaleConstant.ProformaInvoice);
                context.Entity.DocumentType = documentType;
                context.Entity.Amount = 0;
                context.Entity.AdditionalChargesAmount = 0;
                context.Entity.DiscountAmount = 0;
                context.Entity.TaxAmount = 0;
                context.Entity.TotalAmount = 0;
            }
            if (_businessContext.BusinessContext.ContainsKey("DocumentTypeOrder"))
            {
                var documentType = _session.Query<SALSOPDocumentType>()
                                                         .FirstOrDefault(x => x.Name == SaleConstant.Order);
                context.Entity.DocumentType = documentType;
                context.Entity.Amount = 0;
                context.Entity.AdditionalChargesAmount = 0;
                context.Entity.DiscountAmount = 0;
                context.Entity.TaxAmount = 0;
                context.Entity.TotalAmount = 0;
            }
            if (_businessContext.BusinessContext.ContainsKey("DocumentTypeContract"))
            {
                var documentType = _session.Query<SALSOPDocumentType>()
                                                         .FirstOrDefault(x => x.Name == SaleConstant.Contract);
                context.Entity.No = _documentService.GetNextDocumentNumber(SaleConstant.Contract);
                context.Entity.DocumentType = documentType;
                context.Entity.Amount = 0;
                context.Entity.AdditionalChargesAmount = 0;
                context.Entity.DiscountAmount = 0;
                context.Entity.TaxAmount = 0;
                context.Entity.TotalAmount = 0;
            }
            if (_businessContext.BusinessContext.ContainsKey("DocumentTypeInvoice"))
            {

                var documentType = _session.Query<SALSOPDocumentType>()
                                                         .FirstOrDefault(x => x.Name == SaleConstant.Invoice);
                context.Entity.No = _documentService.GetNextDocumentNumber(SaleConstant.Invoice);
                context.Entity.DocumentType = documentType;
                context.Entity.Amount = 0;
                context.Entity.AdditionalChargesAmount = 0;
                context.Entity.DiscountAmount = 0;
                context.Entity.TaxAmount = 0;
                context.Entity.TotalAmount = 0;
            }
            if (_businessContext.BusinessContext.ContainsKey("DocumentTypeInvoiceIssuanceRequest"))
            {

                var documentType = _session.Query<SALSOPDocumentType>()
                                                         .FirstOrDefault(x => x.Name == SaleConstant.InvoiceIssuanceRequest);
                context.Entity.DocumentType = documentType;
                context.Entity.Amount = 0;
                context.Entity.AdditionalChargesAmount = 0;
                context.Entity.DiscountAmount = 0;
                context.Entity.TaxAmount = 0;
                context.Entity.TotalAmount = 0;
            }
            if (_businessContext.BusinessContext.ContainsKey("DocumentTypePriceProposal"))
            {

                var documentType = _session.Query<SALSOPDocumentType>()
                                                         .FirstOrDefault(x => x.Name == SaleConstant.PriceProposal);
                context.Entity.DocumentType = documentType;
                context.Entity.Amount = 0;
                context.Entity.AdditionalChargesAmount = 0;
                context.Entity.DiscountAmount = 0;
                context.Entity.TaxAmount = 0;
                context.Entity.TotalAmount = 0;
            }
        }
    }
}