using System;
using System.Collections.Generic;
using System.Linq;
using BigBang.App.ERPSuiteCore.Domain;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using NHibernate;
using static BigBang.App.Sales.Common.Constants.SaleConstant;

namespace BigBang.App.Sales.GlobalCommand
{
    [ExternalService]
    public class SOPFillProductTotalAmount
    {
        public class ProductTotalAmount
        {
            public decimal TotalPrice { get; set; }
            public decimal Tax { get; set; }
            public decimal Discount { get; set; }
            public decimal AdditionalCharge { get; set; }
        }

        public virtual object FindProductTotalAmount(decimal unitPrice, int quantity, int? id, SuiteCoreProductDetail itemID, int? documentId)
        {
            ISession session = ServiceLocator.Get<ISession>();

            DateTime today = DateTime.Now.Date;

            decimal totalDiscount = 0m, totalTax = 0m, totalAdditionalCharges = 0m;
            int currentItemId = itemID?.Id ?? 0;

            IEnumerable<AdjustmentData> adjustmentsData;
            DateTime effectiveDate = today;
            SALSOPDocument document = null;

            if (id.HasValue && id.Value != 0)
            {
                var item = session.Query<SALSOPDocumentItem>().FirstOrDefault(x => x.ID == id.Value);
                if (item != null && item.ItemID != null)
                {
                    currentItemId = item.ItemID.Id;
                }

                document = item?.Document;

                adjustmentsData = session.Query<SALSOPDocumentAdjustment>()
                    .Where(x => x.DocumentItem.ID == id.Value)
                    .Select(x => new AdjustmentData
                    {
                        TypeName = x.Adjustment.AdjustmentType.Name,
                        Rate = x.Adjustment.Rate,
                        Amount = x.Adjustment.Amount,
                        FromDate = x.Adjustment.FromDate,
                        ToDate = x.Adjustment.ToDate
                    }).ToList();
            }
            else
            {
                adjustmentsData = session.Query<SALSOPAdjustment>()
                    .Where(x => x.IsDefault == true)
                    .Select(x => new AdjustmentData
                    {
                        TypeName = x.AdjustmentType.Name,
                        Rate = x.Rate,
                        Amount = x.Amount,
                        FromDate = x.FromDate,
                        ToDate = x.ToDate
                    }).ToList();
            }

            /* اگر از طریق آیتم به سند نرسیدیم (مثلاً آیتم هنوز ذخیره نشده)، از documentId ورودی استفاده می‌شود. */
            if (document == null && documentId.HasValue && documentId.Value != 0)
            {
                document = session.Query<SALSOPDocument>().FirstOrDefault(d => d.ID == documentId.Value);
            }

            if (document != null)
            {
                effectiveDate = GetEffectiveDocumentDate(document) ?? today;
            }

            /* فقط تعدیلاتی در محاسبه لحاظ شوند که تاریخ مؤثر سند در بازه (FromDate تا ToDate) آن‌ها قرار دارد. */
            adjustmentsData = adjustmentsData
                .Where(a => (!a.FromDate.HasValue || a.FromDate.Value.Date <= effectiveDate) &&
                            (!a.ToDate.HasValue || a.ToDate.Value.Date >= effectiveDate))
                .ToList();

            var effectiveDateTomorrow = effectiveDate.AddDays(1);
            bool isTaxExempt = session.Query<SALSOPProductDetailTaxExemption>()
                .Any(w => w.ProductDetailId.Id == currentItemId
                       && w.FromDate < effectiveDateTomorrow
                       && (w.ToDate == null || w.ToDate >= effectiveDate));

            totalDiscount = CalculateAdjustmentAmount(adjustmentsData, Discount, unitPrice, quantity);
            totalAdditionalCharges = CalculateAdjustmentAmount(adjustmentsData, AdditionalCharges, unitPrice, quantity);

            if (!isTaxExempt)
            {
                totalTax = CalculateAdjustmentAmount(adjustmentsData, Tax, unitPrice, quantity);
            }

            decimal basePrice = unitPrice * quantity;
            decimal finalTotal = basePrice + totalTax + totalAdditionalCharges - totalDiscount;

            return new ProductTotalAmount()
            {
                TotalPrice = finalTotal,
                Tax = totalTax,
                Discount = totalDiscount,
                AdditionalCharge = totalAdditionalCharges
            };
        }

        /// <summary>
        /// تاریخ مؤثر سند: اگر StartDate مقدار داشته باشد همان ملاک است؛
        /// در غیر این صورت از IssueDate استفاده می‌شود (اگر IssueDate مقدار دیفالت/خالی نباشد).
        /// </summary>
        private static DateTime? GetEffectiveDocumentDate(SALSOPDocument document)
        {
            if (document == null)
                return null;

            if (document.StartDate.HasValue)
                return document.StartDate;

            return document.IssueDate != default(DateTime)
                ? (DateTime?)document.IssueDate
                : null;
        }

        private decimal CalculateAdjustmentAmount(IEnumerable<AdjustmentData> adjustments, string adjustmentType, decimal unitPrice, int quantity)
        {
            var targetAdjustments = adjustments.Where(x => x.TypeName == adjustmentType).ToList();

            decimal rateSum = targetAdjustments.Where(x => x.Rate != 0).Sum(x => x.Rate);
            decimal amountSum = targetAdjustments.Where(x => x.Rate == 0).Sum(x => x.Amount);

            return (unitPrice * (rateSum / 100m)) * quantity + amountSum;
        }

        private class AdjustmentData
        {
            public string TypeName { get; set; }
            public decimal Rate { get; set; }
            public decimal Amount { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
        }
    }
}