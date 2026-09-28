namespace BigBang.App.Sales.Reports
{
    public interface IIssuedInvoiceItemReportService
    {
        IList<IssuedInvoiceItemReportDTO> GetIssuedInvoiceItems(
            int documentID);
    }
}

public class IssuedInvoiceItemReportDTO
{
    public int DocumentItemID { get; set; }

    public int DocumentID { get; set; }

    public string ProductName { get; set; }

    public string ProductServiceCode { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal? Amount { get; set; }

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public decimal TotalPrice { get; set; }

    public string PrintDescription { get; set; }

    public string Description { get; set; }
}
