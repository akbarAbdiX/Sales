using BigBang.App.Sales.Domain.Enums;

namespace BigBang.App.Sales.Interfaces.ChangeStateHistoryReportService;

public interface ISOPRPTChangeStateHistoryReportService
{
    List<ChangeStateHistoryReportDTO> GetChangeStateHistoryReport();
}

public class ChangeStateHistoryReportDTO
{
    public int Id { get; set; }
    public string EntityTypeTitle { get; set; }
    public string EntityStateTypeTitle { get; set; }
    public SOPChangeStateHistoryEntityType EntityTypeId { get; set; }
    public SOPChangeStateHistoryEntityStateType EntityStateTypeId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int DocumentNo { get; set; }
}