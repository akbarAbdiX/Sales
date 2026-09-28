using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;

namespace BigBang.App.Sales.GlobalCommand
{
    [ExternalService]
    public class SOPFillDocumentAdjustmentAmount
    {
        public class DocumentAdjustmentAmount
        {
            public decimal AdjustmentAmount { get; set; }
        }

        public virtual object FindDocumentAdjustmentAmount(SALSOPAdjustment adjustment,SALSOPDocumentItem documentItem)
        {
          
            var amount = documentItem.Quantity * documentItem.UnitPrice;
            return new DocumentAdjustmentAmount()
            {
                AdjustmentAmount =(adjustment.Amount== 0m)?
                (amount*adjustment.Rate)/100:
                adjustment.Amount,

            };
        }
    }
}