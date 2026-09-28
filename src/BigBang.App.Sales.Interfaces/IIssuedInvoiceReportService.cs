namespace BigBang.App.Sales.Reports
{
    public interface IIssuedInvoiceReportService
    {
        IList<IssuedInvoiceReportDTO> GetIssuedInvoiceDocuments(
            DateTime? fromDate,
            DateTime? toDate);
    }
}

public class IssuedInvoiceReportDTO
{
    public int DocumentID { get; set; }

    public int InvoiceNo { get; set; }

    public DateTime IssueDate { get; set; }

    public string CustomerName { get; set; }

    public string UniqueTaxIdentifier { get; set; }

    public decimal Amount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string Description { get; set; }
}
