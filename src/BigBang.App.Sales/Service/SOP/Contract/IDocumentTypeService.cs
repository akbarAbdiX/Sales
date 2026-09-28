using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;

namespace BigBang.App.Sales.Service.SOP.Contract
{
    public interface IDocumentTypeService
    {
        SOPChangeStateHistoryEntityType MapToChangeStateHistoryDocumentType(int id);
    }
}
