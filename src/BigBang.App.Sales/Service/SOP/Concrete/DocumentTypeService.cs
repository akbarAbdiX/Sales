using BigBang.App.Sales.Common.Constants;
using BigBang.App.Sales.Common.Exceptions;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;
using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Exceptions;
using static BigBang.App.Sales.Common.Constants.SaleConstant;
using static Stimulsoft.Base.Drawing.StiFontReader;
namespace BigBang.App.Sales.Service.SOP.Concrete
{
    [InternalService(ServiceType = typeof(IDocumentTypeService), InstanceMode = InstanceMode.Scoped)]
    internal class DocumentTypeService : IDocumentTypeService
    {

        private readonly IDocumentTypeRepository _documentTypeRepository;

        public DocumentTypeService(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }

        public SOPChangeStateHistoryEntityType MapToChangeStateHistoryDocumentType(int id)
        {
            var type = _documentTypeRepository.GetFirstOrDefault(x => x.ID == id);
            return (SOPChangeStateHistoryEntityType)type.Code;
        }
    }
}
