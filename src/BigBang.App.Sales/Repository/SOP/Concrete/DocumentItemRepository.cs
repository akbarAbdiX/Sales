using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Repository.Base;
using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using NHibernate;

namespace BigBang.App.Sales.Repository.SOP.Concrete
{
    [InternalService(ServiceType = typeof(IDocumentItemRepository), InstanceMode = InstanceMode.Scoped)]
    internal class DocumentItemRepository: Repository<SALSOPDocumentItem>,IDocumentItemRepository
    {
        public DocumentItemRepository(ISession session) : base(session)
        {

        }
    }
}
