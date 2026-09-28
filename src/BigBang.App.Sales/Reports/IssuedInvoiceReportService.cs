using System;
using System.Collections.Generic;
using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using NHibernate;

namespace BigBang.App.Sales.Reports
{
    [ReportService(ServiceType = typeof(IIssuedInvoiceReportService))]
    public class IssuedInvoiceReportService : IIssuedInvoiceReportService
    {
        private readonly ISession _session;

        public IssuedInvoiceReportService(ISession session)
        {
            _session = session;
        }

        public IList<IssuedInvoiceReportDTO> GetIssuedInvoiceDocuments(
            DateTime? fromDate,
            DateTime? toDate)
        {
            var query = _session.Query<SALSOPDocument>()
                .Where(d =>
                    d.DocumentType != null &&
                    d.DocumentType.Code == (int)SOPDocumentTypeEnum.Invoice &&
                    d.StateID == Domain.Enums.SOPChangeStateHistoryEntityStateType.Confirmed);

            if (fromDate.HasValue)
            {
                query = query.Where(d =>
                    d.IssueDate >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                var endDate = toDate.Value.Date.AddDays(1);

                query = query.Where(d =>
                    d.IssueDate < endDate);
            }

            var result = query
                .OrderByDescending(d => d.IssueDate)
                .ThenByDescending(d => d.ID)
                .Select(d => new IssuedInvoiceReportDTO
                {
                    DocumentID = d.ID,

                    InvoiceNo = d.No,

                    IssueDate = d.IssueDate,

                    CustomerName = d.CustomerID != null
                        ? d.CustomerID.FullName
                        : null,

                    UniqueTaxIdentifier = d.UniqueTaxIdentifier,

                    Amount = d.Amount,

                    DiscountAmount = d.DiscountAmount,

                    TaxAmount = d.TaxAmount,

                    TotalAmount = d.TotalAmount,

                    Description = d.Description
                })
                .ToList();

            return result;
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