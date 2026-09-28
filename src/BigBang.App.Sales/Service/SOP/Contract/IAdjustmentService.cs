using BigBang.App.Sales.Domain;

namespace BigBang.App.Sales.Service.SOP.Contract
{
    public interface IAdjustmentService
    {
        void AdjustmentOnUpdating(SALSOPAdjustment entity);
    }
}
