using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Repository.Base;
using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using NHibernate;

namespace BigBang.App.Sales.Repository.SOP.Concrete
{
    [InternalService(ServiceType = typeof(IDocumentRepository), InstanceMode = InstanceMode.Scoped)]
    internal class DocumentRepository:Repository<SALSOPDocument>,IDocumentRepository
    {
        public DocumentRepository(ISession session) : base(session)
        {

        }
    }
}
