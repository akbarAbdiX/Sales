using BigBang.App.ERPSuiteCore.Domain;
using BigBang.App.ERPSuiteCore.Domain.Enums;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;
using BigBang.WebServer.Common.Attributes;
using BigBang.WebServer.Common.Events;
using BigBang.WebServer.Common.Exceptions;
using NHibernate;

namespace BigBang.App.Sales.ERPSuiteCore.Extended
{
    [EventSubscription(typeof(IEntityUpdatingEvent<SuiteCoreLegalSupplier>))]
    public class SuiteCoreLegalSupplierUpdatingEvent : IEntityUpdatingEvent<SuiteCoreLegalSupplier>
    {
        private readonly ISession _session;

        public SuiteCoreLegalSupplierUpdatingEvent(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public void OnUpdating(IEntityUpdatingEventContext<SuiteCoreLegalSupplier> context)
        {
            if (context?.Entity == null || context.OldEntity == null)
            {
                return;
            }

            bool isDeactivating = context.OldEntity.State == CustomerState.Active &&
                                  context.Entity.State == CustomerState.DeActive;

            if (!isDeactivating)
            {
                return;
            }

            bool hasOpenDocument = _session.Query<SALSOPDocument>()
                .Any(doc => (doc.StateID == SOPChangeStateHistoryEntityStateType.Draft || doc.StateID == SOPChangeStateHistoryEntityStateType.Confirmed) &&
                            doc.CustomerID.Id == context.Entity.Id);

            if (hasOpenDocument)
            {
                throw new BigBangException("امکان غیرفعال‌سازی وجود ندارد؛ زیرا اسناد در جریان (باز) برای این تامین‌کننده حقوقی ثبت شده است.");
            }
        }
    }
}
