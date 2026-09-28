using System.Collections.Generic;
using System.Linq;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using NHibernate;

namespace BigBang.App.Sales.Reports
{
    [ReportService(ServiceType = typeof(IIssuedInvoiceItemReportService))]
    public class IssuedInvoiceItemReportService :
        IIssuedInvoiceItemReportService
    {
        private readonly ISession _session;

        public IssuedInvoiceItemReportService(ISession session)
        {
            _session = session;
        }

        public IList<IssuedInvoiceItemReportDTO> GetIssuedInvoiceItems(
            int documentID)
        {
            var result = _session.Query<SALSOPDocumentItem>()
                .Where(item =>
                    item.Document != null &&
                    item.Document.ID == documentID)
                .Select(item => new IssuedInvoiceItemReportDTO
                {
                    DocumentItemID = item.ID,

                    DocumentID = documentID,

                    ProductName = item.ItemID != null
                        ? item.ItemID.ProductDetailName
                        : null,

                    ProductServiceCode = item.ItemID != null
                        ? item.ItemID.ProductServiceCode
                        : null,

                    UnitPrice = item.UnitPrice,

                    Quantity = item.Quantity,

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
    }
}