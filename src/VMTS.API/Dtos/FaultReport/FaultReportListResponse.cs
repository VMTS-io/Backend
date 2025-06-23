using VMTS.API.Dtos.TripReport;

namespace VMTS.API.Dtos;

public class FaultReportListResponse
{
    public int StatusCode { get; set; }
    public IReadOnlyList<FaultReportResponse> FaultReports { get; set; }
}
