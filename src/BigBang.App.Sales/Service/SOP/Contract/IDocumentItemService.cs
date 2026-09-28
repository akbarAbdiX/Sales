using BigBang.App.Sales.Domain;

namespace BigBang.App.Sales.Service.SOP.Contract
{
    public interface IDocumentItemService
    {
        SALSOPDocumentItem GetById(int Id);
        bool ValidateToUpdate(SALSOPDocumentItem entity);
        void DocumentItemOnAdding(SALSOPDocumentItem entity);
        void DocumentItemOnUpdating(SALSOPDocumentItem entity);
        void DocumentItemOnDeleting(SALSOPDocumentItem entity);
    }
}
