using BigBang.App.Sales.Domain;

namespace BigBang.App.Sales.Service.SOP.Contract
{
    public interface IDocumentAdjustmentService
    {
        void DocumentAdjustmentOnAdding(SALSOPDocumentAdjustment entity);
        void DocumentAdjustmentOnUpdating(SALSOPDocumentAdjustment entity);
        void DocumentAdjustmentOnDeleting(SALSOPDocumentAdjustment entity);

    }
}
