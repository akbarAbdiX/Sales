using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Repository.Base;
using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using NHibernate;

namespace BigBang.App.Sales.Repository.SOP.Concrete
{
    [InternalService(ServiceType = typeof(IDocumentAdjustmentRepository), InstanceMode = InstanceMode.Scoped)]
   internal class DocumentAdjustmentRepository :Repository<SALSOPDocumentAdjustment>,IDocumentAdjustmentRepository
    {
        public DocumentAdjustmentRepository(ISession session) : base(session)
        {

        }
    }
}
