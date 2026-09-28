using System;
using BigBang.App.Sales.Domain;

namespace BigBang.App.Sales.Businesses.SOPProductAndServiceRate
{
    internal static class ProductAndServiceRateValidity
    {
        private const int ProformaInvoiceTypeId = 1;
        private const int InvoiceTypeId = 4;

        public static bool IsInvoiceOrProforma(SALSOPDocument document)
        {
            var typeId = document?.DocumentType?.ID;
            return typeId == ProformaInvoiceTypeId || typeId == InvoiceTypeId;
        }

        public static bool IsValidOn(SALSOPProductAndServiceRate rate, DateTime documentDate)
        {
            if (rate == null)
                return false;

            var date = documentDate.Date;
            if (rate.StartDate.Date > date)
                return false;
            if (rate.EndDate.HasValue && rate.EndDate.Value.Date < date)
                return false;

            return true;
        }

        public static DateTime GetDocumentDate(SALSOPDocument document)
        {
            if (document == null || document.IssueDate == default)
                return DateTime.Now.Date;

            return document.IssueDate.Date;
        }
    }
}