namespace BigBang.App.sales.Interfaces
{
    public interface IDocumentPrintService
    {
        IList<SOPDocumentPrintDTO> GetDocuments(int documentID);
    }

    public class SOPDocumentPrintDTO
    {
        public int DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerRegistrationNumber { get; set; }
        public string CustomerEconomicCode { get; set; }
        public string CustomerFaxNumber { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerAddress { get; set; }
        public string SellerName { get; set; }
        public string SellerRegistrationNumber { get; set; }
        public string SellerEconomicCode { get; set; }
        public string SellerFaxNumber { get; set; }
        public string SellerNumber { get; set; }
        public string SellerAddress { get; set; }
        public string DocumentItemTitle { get; set; }
        public int DocumentItemNumber { get; set; }
        public int DocumentTypeID { get; set; }
        public string DocumentItemDescription { get; set; }
        public long TotalPrice { get; set; }
        public long UnitPrice { get; set; }
        public long TaxAmount { get; set; }
        public string DocumentDateTime { get; set; }
        public string UniqueTaxIdentifier { get; set; }
        public int BranchCode { get; set; }
        public string SellerPostalCode { get; set; }
        public string CustomerPostalCode { get; set; }
        public long DocumentItemAdditionalChargesAmount { get; set; }
        public string CustomerNationalCode { get; set; }
        public string ProductServiceCode { get; set; }
        public string SettlementMethod { get; set; }
        public decimal Discount { get; set; }
        public long? VatShare { get; set; }
        public long? PaymentTermAmount { get; set; }
        public bool HasContract { get; set; }
        public string PaymentTermsType { get; set; }
        public long? InvoicePaymentTermAmount { get; set; }
        public long FinalPaymentAmount { get; set; }
    }
}
