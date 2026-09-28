public interface IUninvoicedProformaItemReportService
{
    IList<UninvoicedProformaItemReportDTO> GetItems(
        int proformaInvoiceID);
}


public class UninvoicedProformaItemReportDTO
{
    public int ProformaInvoiceID { get; set; }

    public int ProformaInvoiceNo { get; set; }

    public string ProductName { get; set; }

    public string ProductServiceCode { get; set; }

    public decimal? Amount { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public decimal TotalPrice { get; set; }

    public string PrintDescription { get; set; }

    public string Description { get; set; }
}