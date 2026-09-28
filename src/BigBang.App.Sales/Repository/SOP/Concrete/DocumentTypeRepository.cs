using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Repository.Base;
using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using NHibernate;

namespace BigBang.App.Sales.Repository.SOP.Concrete
{
    [InternalService(ServiceType = typeof(IDocumentTypeRepository), InstanceMode = InstanceMode.Scoped)]
    internal class DocumentTypeRepository : Repository<SALSOPDocumentType>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(ISession session) : base(session)
        {

        }

    }
}
