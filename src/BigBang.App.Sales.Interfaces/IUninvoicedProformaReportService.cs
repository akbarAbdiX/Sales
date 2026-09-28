using BigBang.App.Sales.Domain;

public interface IUninvoicedProformaReportService
{
    IList<UninvoicedProformaReportDTO> GetDocuments(
        DateTime? fromDate,
        DateTime? toDate);
}


public class UninvoicedProformaReportDTO
{
    public int ProformaInvoiceID { get; set; }

    public SALSOPDocument ProformaInvoice { get; set; }

    public int ProformaInvoiceNo { get; set; }

    public DateTime ProformaInvoiceDate { get; set; }

    public string CustomerName { get; set; }

    public decimal TotalAmount { get; set; }

    public string Description { get; set; }

    public int? CurrentStageID { get; set; }

    public string CurrentStage { get; set; }
}