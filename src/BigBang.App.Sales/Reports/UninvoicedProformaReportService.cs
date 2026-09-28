using System;
using System.Collections.Generic;
using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using NHibernate;

namespace BigBang.App.Sales.Reports
{
    [ReportService(ServiceType = typeof(IUninvoicedProformaReportService))]
    public class UninvoicedProformaReportService
        : IUninvoicedProformaReportService
    {
        private readonly ISession _session;

        private const int MaxChainDepth = 50;

        public UninvoicedProformaReportService()
        {
        }

        public UninvoicedProformaReportService(ISession session)
        {
            _session = session;
        }

        public IList<UninvoicedProformaReportDTO> GetDocuments(
            DateTime? fromDate,
            DateTime? toDate)
        {
            if (_session == null)
                throw new InvalidOperationException(
                    "NHibernate session is not initialized.");


            var query = _session.Query<SALSOPDocument>()
                .Where(d =>
                    d.DocumentType != null &&
                    d.DocumentType.Code ==
                    (int)SOPDocumentTypeEnum.ProformaInvoice);

            if (fromDate.HasValue)
            {
                var startDate = fromDate.Value.Date;

                query = query.Where(d =>
                    d.IssueDate >= startDate);
            }


            if (toDate.HasValue)
            {
                var endDate = toDate.Value.Date.AddDays(1);

                query = query.Where(d =>
                    d.IssueDate < endDate);
            }


            var proformas = query
                .OrderBy(d => d.IssueDate)
                .ThenBy(d => d.ID)
                .ToList();

            var result =
                new List<UninvoicedProformaReportDTO>();

            foreach (var proforma in proformas)
            {

                var lastDocument =
                    GetLastDocumentInChain(proforma.ID);

                if (lastDocument != null &&
                    lastDocument.DocumentTypeID ==
                    (int)SOPDocumentTypeEnum.Invoice)
                {
                    continue;
                }

                result.Add(
                    new UninvoicedProformaReportDTO
                    {
                        ProformaInvoice = proforma,

                        ProformaInvoiceID =
                            proforma.ID,

                        ProformaInvoiceNo =
                            proforma.No,

                        ProformaInvoiceDate =
                            proforma.IssueDate,

                        CustomerName =
                            proforma.CustomerID != null
                                ? proforma.CustomerID.FullName
                                : null,

                        TotalAmount =
                            proforma.TotalAmount,

                        Description =
                            proforma.Description,

                        CurrentStageID =
                            lastDocument?.DocumentTypeID,

                        CurrentStage =
                            lastDocument?.DocumentTypeTitle
                    });
            }

            return result;
        }

        private DocumentChainInfo GetLastDocumentInChain(
            int proformaID)
        {
            var currentDocumentID = proformaID;

            DocumentChainInfo lastDocument = null;


            var visitedDocuments =
                new HashSet<int>();

            var depth = 0;

            while (depth < MaxChainDepth)
            {
                depth++;


                if (!visitedDocuments.Add(currentDocumentID))
                {
                    break;
                }


                var child = _session
                    .Query<SALSOPDocument>()
                    .Where(d =>
                        d.Document != null &&
                        d.Document.ID == currentDocumentID)
                    .OrderByDescending(d => d.ID)
                    .Select(d =>
                        new DocumentChainInfo
                        {
                            DocumentID = d.ID,

                            DocumentTypeID =
                                d.DocumentType != null
                                    ? (int?)d.DocumentType.Code
                                    : null,

                            DocumentTypeTitle =
                                d.DocumentType != null
                                    ? d.DocumentType.Title
                                    : null,

                            ParentDocumentID =
                                d.Document != null
                                    ? (int?)d.Document.ID
                                    : null
                        })
                    .FirstOrDefault();


                if (child == null)
                {
                    break;
                }


                lastDocument = child;


                currentDocumentID =
                    child.DocumentID;
            }


            if (lastDocument == null)
            {
                lastDocument = GetDocumentInfo(
                    proformaID);
            }

            return lastDocument;
        }


        private DocumentChainInfo GetDocumentInfo(
            int documentID)
        {
            return _session
                .Query<SALSOPDocument>()
                .Where(d => d.ID == documentID)
                .Select(d =>
                    new DocumentChainInfo
                    {
                        DocumentID = d.ID,

                        DocumentTypeID =
                            d.DocumentType != null
                                ? (int?)d.DocumentType.Code
                                : null,

                        DocumentTypeTitle =
                            d.DocumentType != null
                                ? d.DocumentType.Title
                                : null,

                        ParentDocumentID =
                            d.Document != null
                                ? (int?)d.Document.ID
                                : null
                    })
                .FirstOrDefault();
        }

        private class DocumentChainInfo
        {
            public int DocumentID { get; set; }

            public int? DocumentTypeID { get; set; }

            public string DocumentTypeTitle { get; set; }

            public int? ParentDocumentID { get; set; }
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