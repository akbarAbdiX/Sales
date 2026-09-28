using BigBang.App.Sales.Domain;
using BigBang.WebServer.Common.Attributes;

namespace BigBang.App.Sales.GlobalCommand
{
    [ExternalService]
    public class SOPFillContractPaymentAmountGlobalCommandService
    {
        public SOPFillContractPaymentAmountGlobalCommandService()
        {
            
        }
        public class ContractPaymentAmount
        {
            public decimal Amount { get; set; }
        }

        public virtual object FindContractPaymentAmount(SALSOPDocument document, decimal perAmount)
        {
            return new ContractPaymentAmount()
            {
                Amount = document.TotalAmount / (perAmount * 100)
            };
        }
    }
}