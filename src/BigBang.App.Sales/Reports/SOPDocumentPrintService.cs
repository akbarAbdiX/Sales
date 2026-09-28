using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BigBang.App.sales.Interfaces;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;
using NHibernate;

namespace BigBang.App.Sales.Reports
{
    [ReportService(ServiceType = typeof(IDocumentPrintService))]
    public class SOPDocumentPrintService : IDocumentPrintService
    {
        private readonly ISession _session;

        public SOPDocumentPrintService(ISession session)
        {
            _session = session;
        }

        public IList<SOPDocumentPrintDTO> GetDocuments(int documentID)
        {

            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);

            var currentVatShare = _session.Query<SALSOPPaymentVatShare>()
                .Where(v => (v.FromDate == null || v.FromDate < tomorrow)
                         && (v.ToDate == null || v.ToDate >= today))
                .OrderByDescending(v => v.Id)
                .Select(v => (decimal?)v.VatShare)
                .FirstOrDefault();

            var rawData = (from docItem in _session.Query<SALSOPDocumentItem>()
                           where docItem.Document != null && docItem.Document.ID == documentID
                           select new {
                               InvoicePaymentTermAmount = docItem.Document.PaymentTermAmount,
                               PaymentTermsType = docItem.Document.PaymentTermsType,
                               DocumentNo = docItem.Document.No,
                               IssueDate = docItem.Document.IssueDate,
                               AccountEconomicCode = docItem.Document.CustomerID != null
                                   ? docItem.Document.CustomerID.Accounts.Select(a => a.EconomicCode).FirstOrDefault() : null,
                               SupplierEconomicCode = docItem.Document.CustomerID != null
                                   ? docItem.Document.CustomerID.LegalSuppliers.Select(l => l.EconomicCode).FirstOrDefault() : null,
                               SellerName = docItem.Document.Seller != null ? docItem.Document.Seller.Name : null,
                               SellerEconomicCode = docItem.Document.Seller != null ? docItem.Document.Seller.EconomicCode : null,
                               SellerAddress = docItem.Document.Seller != null ? docItem.Document.Seller.Address : null,
                               SellerFaxNumber = docItem.Document.Seller != null ? docItem.Document.Seller.FaxNumber : null,
                               SellerNumber = docItem.Document.Seller != null ? docItem.Document.Seller.PhoneNumber : null,
                               SellerRegistrationNumber = docItem.Document.Seller != null ? docItem.Document.Seller.RegistrationNumber : null,
                               SellerPostalCode = docItem.Document.Seller != null ? docItem.Document.Seller.PostalCode : null,
                               BranchCode = docItem.Document.Seller != null ? docItem.Document.Seller.BranchCode : null,
                               UniqueTaxIdentifier = docItem.Document.UniqueTaxIdentifier,
                               CustomerName = docItem.Document.CustomerID != null ? docItem.Document.CustomerID.FullName : null,
                               CustomerFaxNumber = docItem.Document.CustomerID != null ? docItem.Document.CustomerID.FaxNumber : null,
                               CustomerNumber = docItem.Document.CustomerID != null ? docItem.Document.CustomerID.CustomerNumber : null,
                               CustomerNationalCode = docItem.Document.CustomerID != null ? docItem.Document.CustomerID.NationalCode : null,
                               AccountRegistrationNumber = docItem.Document.CustomerID != null
                                   ? docItem.Document.CustomerID.Accounts.Select(a => a.RegistrationNumber).FirstOrDefault() : null,
                               SupplierRegistrationNumber = docItem.Document.CustomerID != null
                                   ? docItem.Document.CustomerID.LegalSuppliers.Select(l => l.RegistrationNumber).FirstOrDefault() : null,
                               CustomerAddress = docItem.Document.CustomerID != null
                                   ? docItem.Document.CustomerID.Addresses.Select(ad => ad.FullAddress).FirstOrDefault() : null,
                               CustomerPostalCode = docItem.Document.CustomerID != null
                                   ? docItem.Document.CustomerID.Addresses.Select(ad => ad.PostalCode).FirstOrDefault() : null,
                               ProductServiceCode = docItem.ItemID != null ? docItem.ItemID.ProductServiceCode : null,
                               DocumentItemTitle = docItem.ItemID != null ? docItem.ItemID.ProductDetailName : null,
                               DocumentTypeID = docItem.Document.DocumentType != null ? docItem.Document.DocumentType.Code : 0,
                               DocumentItemAdditionalChargesAmount = docItem.Document.AdditionalChargesAmount,
                               PrintDescription = docItem.PrintDescription,
                               DocumentItemDescription = docItem.Description,
                               TotalPrice = docItem.TotalPrice,
                               UnitPrice = docItem.UnitPrice,
                               Tax = docItem.Tax,
                               AdditionalCharge = docItem.AdditionalCharge,
                               Number = docItem.Quantity,
                               Discount = docItem.Discount,
                               VatShare = currentVatShare,

                               DocumentID = docItem.Document.ID
                           }).ToList();

            return rawData.Select(r =>
            {
                var settlementData = GetContractSettlementData(r.DocumentID);
                bool hasContractForThisDoc = settlementData.HasContract;
                bool invoiceHasOwnAmount = r.InvoicePaymentTermAmount.HasValue && r.InvoicePaymentTermAmount.Value != 0;
                decimal finalPaymentAmount = invoiceHasOwnAmount
                    ? r.InvoicePaymentTermAmount.Value
                    : (settlementData.PaymentTermAmount ?? 0);
                string PaymentTermsTypeName = r.PaymentTermsType == Domain.Enums.SOPSettlementMethod.Cash ? "نقد"
                                            : r.PaymentTermsType == Domain.Enums.SOPSettlementMethod.Credit ? "نسیه"
                                            : r.PaymentTermsType.ToString() ?? string.Empty;
                string settlementMethodText = settlementData.SettlementMethod == 1 ? "نقد"
                                            : settlementData.SettlementMethod == 2 ? "نسیه"
                                            : settlementData.SettlementMethod?.ToString() ?? PaymentTermsTypeName;
                return new SOPDocumentPrintDTO
                {

                    Discount = r.Discount,
                    DocumentNo = r.DocumentNo,
                    DocumentDate = ToPersianDate(r.IssueDate),
                    CustomerEconomicCode = r.AccountEconomicCode?.ToString() ?? r.SupplierEconomicCode?.ToString(),
                    ProductServiceCode = r.ProductServiceCode,
                    SellerName = r.SellerName,
                    SellerEconomicCode = r.SellerEconomicCode?.ToString(),
                    SellerAddress = r.SellerAddress?.ToString(),
                    SellerFaxNumber = r.SellerFaxNumber?.ToString(),
                    SellerNumber = r.SellerNumber?.ToString(),
                    SellerRegistrationNumber = r.SellerRegistrationNumber?.ToString(),
                    UniqueTaxIdentifier = r.UniqueTaxIdentifier,
                    BranchCode = r.BranchCode.HasValue ? (int)r.BranchCode.Value : 0,
                    CustomerPostalCode = r.CustomerPostalCode,
                    CustomerName = r.CustomerName,
                    CustomerFaxNumber = r.CustomerFaxNumber,
                    CustomerNumber = r.CustomerNumber,
                    CustomerNationalCode = r.CustomerNationalCode,
                    SellerPostalCode = r.SellerPostalCode?.ToString(),
                    DocumentTypeID = r.DocumentTypeID,
                    CustomerRegistrationNumber = r.AccountRegistrationNumber ?? r.SupplierRegistrationNumber,
                    CustomerAddress = r.CustomerAddress,
                    DocumentItemNumber = r.Number,
                    VatShare = (long?)r.VatShare,
                    DocumentItemTitle = !string.IsNullOrWhiteSpace(r.PrintDescription) ? r.PrintDescription : r.DocumentItemTitle,
                    DocumentItemDescription = r.DocumentItemDescription,
                    TotalPrice = (long)r.TotalPrice,
                    UnitPrice = (long)r.UnitPrice,
                    DocumentItemAdditionalChargesAmount = (long)r.DocumentItemAdditionalChargesAmount,
                    DocumentDateTime = ToPersianDateTime(r.IssueDate),
                    TaxAmount = (long)r.Tax,
                    SettlementMethod = settlementMethodText,
                    PaymentTermAmount = (long)(settlementData.PaymentTermAmount ?? 0),
                    PaymentTermsType = PaymentTermsTypeName,
                    HasContract = hasContractForThisDoc,
                    InvoicePaymentTermAmount = r.InvoicePaymentTermAmount,
                    FinalPaymentAmount = (long)finalPaymentAmount
                };
            }).ToList();
        }

        private static string ToPersianDate(DateTime? date)
        {
            if (!date.HasValue || date.Value == DateTime.MinValue)
                return string.Empty;

            var pc = new PersianCalendar();
            return string.Format("{0:0000}/{1:00}/{2:00}",
                pc.GetYear(date.Value),
                pc.GetMonth(date.Value),
                pc.GetDayOfMonth(date.Value));
        }

        public (byte? SettlementMethod, decimal? PaymentTermAmount, bool HasContract) GetContractSettlementData(int? documentId)
        {
            if (documentId == null) return (null, null, false);

            int? currentDocId = documentId;
            int maxDepth = 10;
            int currentDepth = 0;

            while (currentDocId != null && currentDepth < maxDepth)
            {
                currentDepth++;

                var doc = _session.Query<SALSOPDocument>()
                                  .Where(d => d.ID == currentDocId)
                                  .Select(d => new {
                                      d.ID,
                                      TypeCode = d.DocumentType != null ? (int?)d.DocumentType.Code : null,
                                      ParentId = d.Document != null ? (int?)d.Document.ID : null,
                                      PaymentTerms = d.SOPPaymentTerms
                                  })
                                  .FirstOrDefault();

                if (doc == null)
                    break;

                bool isContract = doc.TypeCode.HasValue && doc.TypeCode.Value == (int)SOPDocumentTypeEnum.Contract;

                if (isContract)
                {
                    if (doc.PaymentTerms != null && doc.PaymentTerms.Any())
                    {

                        decimal totalAmount = doc.PaymentTerms.Sum(pt => (decimal?)(pt.Amount) ?? 0m);
                        var settlementMethod = doc.PaymentTerms
                            .Select(pt => pt.SettlementMethod)
                            .FirstOrDefault(sm => sm != null);

                        return (
                            (byte?)(settlementMethod != null ? (byte)settlementMethod.Value : (byte?)null),
                            totalAmount,
                            true
                        );
                    }

                    return (null, null, true);
                }

                currentDocId = doc.ParentId;
            }

            return (null, null, false);
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

        private static string ToPersianDateTime(DateTime? date)
        {
            if (!date.HasValue || date.Value == DateTime.MinValue)
                return string.Empty;

            var pc = new PersianCalendar();
            return $"{pc.GetYear(date.Value):0000}/{pc.GetMonth(date.Value):00}/{pc.GetDayOfMonth(date.Value):00} {date.Value.Hour:00}:{date.Value.Minute:00}";
        }
    }
}