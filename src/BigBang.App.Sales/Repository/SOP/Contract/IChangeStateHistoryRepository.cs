using System.Collections.Generic;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Interfaces.ChangeStateHistoryReportService;
using BigBang.App.Sales.Repository.Base;

namespace BigBang.App.Sales.Repository.SOP.Contract;

public interface IChangeStateHistoryRepository : IRepository<SALSOPChangeStateHistory>
{
    List<ChangeStateHistoryReportDTO> GetChangeStateHistoryReport();

}