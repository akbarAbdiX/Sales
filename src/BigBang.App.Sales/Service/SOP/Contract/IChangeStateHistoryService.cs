using System.Collections.Generic;
using BigBang.App.Sales.Domain;
using BigBang.App.Sales.Domain.Enums;
using BigBang.App.Sales.Interfaces.ChangeStateHistoryReportService;

namespace BigBang.App.Sales.Service.SOP.Contract;

public interface IChangeStateHistoryService
{
    void Add(SOPChangeStateHistoryEntityType EntityType,
             long entityId,
             SOPChangeStateHistoryEntityStateType EntityStateType,
             string comment);

    SALSOPChangeStateHistory GetCurrentStateByEntity(SOPChangeStateHistoryEntityType entityTypeId, long entityId);
    void Validate(SALSOPChangeStateHistory document);
    void ValidateContractNumber(SALSOPChangeStateHistory document);

    List<ChangeStateHistoryReportDTO> GetChangeStateHistoryReport();

}