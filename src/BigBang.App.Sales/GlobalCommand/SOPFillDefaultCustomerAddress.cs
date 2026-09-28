using System.Linq;
using BigBang.App.ERPSuiteCore.Domain;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using NHibernate;

namespace BigBang.App.Sales.GlobalCommand
{
    [ExternalService]
    public class SOPFillDefaultCustomerAddress
    {
        public SOPFillDefaultCustomerAddress()
        {

        }
        public class DefaultCustomerAddress
        {

            public SuiteCoreAddress CustomerAddress { get; set; }
        }

        public virtual object FindDefaultCustomerAddress(SuiteCoreParty customerId)
        {
            ISession session = ServiceLocator.Get<ISession>();

            return new DefaultCustomerAddress()
            {
                CustomerAddress = session.Query<SuiteCoreAddress>()
                    .Where(x => x.Party.Id == customerId.Id && x.Isdefault == true)
                    .FirstOrDefault()
            };
        }
    }
}
