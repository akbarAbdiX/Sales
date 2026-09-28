using System.Collections.Generic;
using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using NHibernate;

namespace BigBang.App.Sales.Reports
{
    [ReportService(ServiceType = typeof(IUninvoicedProformaItemReportService))]
    public class UninvoicedProformaItemReportService :
        IUninvoicedProformaItemReportService
    {
        private readonly ISession _session;

        public UninvoicedProformaItemReportService(ISession session)
        {
            _session = session;
        }

        public IList<UninvoicedProformaItemReportDTO> GetItems(
            int proformaInvoiceID)
        {

            var lastDocument = GetLastChildDocument(proformaInvoiceID);

            if (lastDocument == null)
                return new List<UninvoicedProformaItemReportDTO>();

            if (lastDocument.DocumentTypeID ==
                (int)SOPDocumentTypeEnum.Invoice)
            {
                return new List<UninvoicedProformaItemReportDTO>();
            }


            var document = _session.Query<SALSOPDocument>()
                .Where(d => d.ID == proformaInvoiceID)
                .Select(d => new {
                    d.ID,
                    d.No
                })
                .FirstOrDefault();

            if (document == null)
                return new List<UninvoicedProformaItemReportDTO>();


            var result = _session.Query<SALSOPDocumentItem>()
                .Where(item =>
                    item.Document != null &&
                    item.Document.ID == proformaInvoiceID)
                .Select(item => new UninvoicedProformaItemReportDTO
                {
                    ProformaInvoiceID = proformaInvoiceID,

                    ProformaInvoiceNo = document.No,

                    ProductName = item.ItemID != null
                        ? item.ItemID.ProductDetailName
                        : null,

                    ProductServiceCode = item.ItemID != null
                        ? item.ItemID.ProductServiceCode
                        : null,

                    Quantity = item.Quantity,

                    UnitPrice = item.UnitPrice,

                    Amount = item.Amount,

                    Discount = item.Discount,

                    Tax = item.Tax,

                    TotalPrice = item.TotalPrice,

                    PrintDescription = item.PrintDescription,

                    Description = item.Description
                })
                .ToList();

            return result;
        }

        private DocumentChainInfo GetLastChildDocument(int proformaID)
        {
            int currentDocumentID = proformaID;

            DocumentChainInfo lastDocument = null;

            int maxDepth = 20;
            int depth = 0;

            while (depth < maxDepth)
            {
                depth++;

                var child = _session.Query<SALSOPDocument>()
                    .Where(d =>
                        d.Document != null &&
                        d.Document.ID == currentDocumentID)
                    .OrderByDescending(d => d.ID)
                    .Select(d => new DocumentChainInfo
                    {
                        DocumentID = d.ID,

                        DocumentTypeID = d.DocumentType != null
                            ? (int?)d.DocumentType.Code
                            : null,

                        DocumentTypeTitle = d.DocumentType != null
                            ? d.DocumentType.Title
                            : null
                    })
                    .FirstOrDefault();

                if (child == null)
                    break;

                lastDocument = child;

                currentDocumentID = child.DocumentID;
            }

            if (lastDocument == null)
            {
                lastDocument = _session.Query<SALSOPDocument>()
                    .Where(d => d.ID == proformaID)
                    .Select(d => new DocumentChainInfo
                    {
                        DocumentID = d.ID,

                        DocumentTypeID = d.DocumentType != null
                            ? (int?)d.DocumentType.Code
                            : null,

                        DocumentTypeTitle = d.DocumentType != null
                            ? d.DocumentType.Title
                            : null
                    })
                    .FirstOrDefault();
            }

            return lastDocument;
        }

        private class DocumentChainInfo
        {
            public int DocumentID { get; set; }

            public int? DocumentTypeID { get; set; }

            public string DocumentTypeTitle { get; set; }
        }

        public enum SOPDocumentTypeEnum
        {
            ProformaInvoice = 1,
            Order = 2,
            Contract = 3,
            Invoice = 4,
            InvoiceIssuanceRequest = 5,
            PriceProposal = 6
        }
    }
}