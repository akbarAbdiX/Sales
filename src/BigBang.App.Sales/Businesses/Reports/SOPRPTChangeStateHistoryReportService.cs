using System;
using System.Collections.Generic;
using BigBang.App.Sales.Interfaces.ChangeStateHistoryReportService;
using BigBang.App.Sales.Service.SOP.Contract;
using BigBang.WebServer.Common.Attributes;

namespace BigBang.App.Accounting.Business.Reports;

[ReportService(ServiceType = typeof(ISOPRPTChangeStateHistoryReportService))]
public class SOPRPTChangeStateHistoryReportService : ISOPRPTChangeStateHistoryReportService
{

    private readonly IChangeStateHistoryService _changeStateHistoryService;
    public SOPRPTChangeStateHistoryReportService(IChangeStateHistoryService changeStateHistoryService)
    {
        _changeStateHistoryService = changeStateHistoryService ?? throw new ArgumentNullException(nameof(changeStateHistoryService));
    }

    public List<ChangeStateHistoryReportDTO> GetChangeStateHistoryReport()
    {   
        return _changeStateHistoryService.GetChangeStateHistoryReport();
    }
}