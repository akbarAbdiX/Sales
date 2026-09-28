using System.Collections.Generic;
using System.Linq;
using BigBang.App.LocalAuthentication.Domain;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;
using BigBang.App.Sales.Interfaces.ChangeStateHistoryReportService;
using BigBang.App.Sales.Repository.Base;
using BigBang.App.Sales.Repository.SOP.Contract;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.Attributes;
using NHibernate;
using NHibernate.Mapping;
using NHibernate.Transform;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BigBang.App.Sales.Repository.SOP.Concrete;

[InternalService(ServiceType = typeof(IChangeStateHistoryRepository), InstanceMode = InstanceMode.Scoped)]
internal class ChangeStateHistoryRepository : Repository<SALSOPChangeStateHistory>, IChangeStateHistoryRepository
{
    public ChangeStateHistoryRepository(ISession session) : base(session)
    {

    }

    public List<ChangeStateHistoryReportDTO> GetChangeStateHistoryReport()
    {
        ChangeStateHistoryReportDTO changeStateHistoryPlus = null;
        SALSOPChangeStateHistory changeStateHistory = null;
        LAUser lauser = null;
        SALSOPDocument document = null;

        IList<ChangeStateHistoryReportDTO> reportResultModels = _session.QueryOver(() => changeStateHistory).Where(() => changeStateHistory.IsCurrent == true)
            .JoinEntityAlias(() => lauser, () => lauser.Id == changeStateHistory.UserId)
            .JoinEntityAlias(() => document, () => document.ID == changeStateHistory.EntityId)
            .SelectList(x => x
                .Select(() => changeStateHistory.Id).WithAlias(() => changeStateHistoryPlus.Id)
                .Select(() => changeStateHistory.EntityTypeTitle).WithAlias(() => changeStateHistoryPlus.EntityTypeTitle)
                .Select(() => changeStateHistory.EntityStateTypeTitle).WithAlias(() => changeStateHistoryPlus.EntityStateTypeTitle)
                .Select(() => changeStateHistory.CreatedDate).WithAlias(() => changeStateHistoryPlus.CreatedDate)
                .Select(() => lauser.Username).WithAlias(() => changeStateHistoryPlus.UserName)
                .Select(() => document.No).WithAlias(() => changeStateHistoryPlus.DocumentNo)
            )
            .TransformUsing(Transformers.AliasToBean<ChangeStateHistoryReportDTO>())
            .List<ChangeStateHistoryReportDTO>();

        return reportResultModels.ToList();
    }
}
