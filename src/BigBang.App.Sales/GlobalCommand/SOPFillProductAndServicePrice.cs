using System.Linq;
using BigBang.App.ERPSuiteCore.Domain;
using BigBang.App.Sales.Businesses.SOPProductAndServiceRate;
using BigBang.App.Sales.Common.Exceptions;
using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;

namespace BigBang.App.Sales.GlobalCommand
{
    [ExternalService]
    public class SOPFillProductAndServicePrice
    {
        public SOPFillProductAndServicePrice() { }
        public class ProductUnitPrice
        {
            public decimal UnitPrice { get; set; }
            public SALSOPProductAndServiceRate ProductAndServiceRate { get; set; }
        }
        public virtual object FindProductUnitPrice(SuiteCoreProductDetail item, SALSOPDocument document)
        {
            ISession session = ServiceLocator.Get<ISession>();
            var productAndServiceRate = FindValidRateOnDocumentDate(session, item, document);

            if (productAndServiceRate == null)
            {
                if (session.Query<SALSOPProductAndServiceRate>().Any(x => x.ItemID.Id == item.Id))
                    throw Exceptions.Raise(ExceptionCode.ProductAndServiceRateNotValidOnDocumentDate);

                return EmptyRate();
            }

            if (!ProductAndServiceRateValidity.IsInvoiceOrProforma(document)
                && document?.EndDate != null
                && productAndServiceRate.EndDate != null
                && document.EndDate.Value.Date > productAndServiceRate.EndDate.Value.Date)
            {
                throw new BigBangException("تاریخ مبلغ کالا/خدمت منقضی شده");
            }

            return new ProductUnitPrice
            {
                UnitPrice = productAndServiceRate.BasePrice,
                ProductAndServiceRate = productAndServiceRate
            };
        }

        private static SALSOPProductAndServiceRate FindValidRateOnDocumentDate(ISession session, SuiteCoreProductDetail item, SALSOPDocument document)
        {
            var dayStart = ProductAndServiceRateValidity.GetDocumentDate(document);
            var nextDay = dayStart.AddDays(1);

            return session.Query<SALSOPProductAndServiceRate>()
                .Where(x => x.ItemID.Id == item.Id
                    && x.StartDate < nextDay
                    && (x.EndDate == null || x.EndDate >= dayStart))
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();
        }

        private static ProductUnitPrice EmptyRate() => new()
        {
            UnitPrice = 0m,
            ProductAndServiceRate = new SALSOPProductAndServiceRate()
        };
    }
}