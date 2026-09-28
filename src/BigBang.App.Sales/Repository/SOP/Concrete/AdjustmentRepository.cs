using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Repository.Base;
using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.WebServer.Common.Attributes;
using NHibernate;

namespace BigBang.App.Sales.Repository.SOP.Concrete
{

    [InternalService(ServiceType = typeof(IAdjustmentRepository), InstanceMode = InstanceMode.Scoped)]
   internal class AdjustmentRepository : Repository<SALSOPAdjustment>, IAdjustmentRepository
    {
        public AdjustmentRepository(ISession session):base(session)
        {

        }
    }
}
