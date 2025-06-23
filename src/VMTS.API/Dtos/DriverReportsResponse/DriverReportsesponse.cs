using VMTS.API.Dtos.TripReport;

namespace VMTS.API.Dtos.DriverReportsResponse;

public class DriverReportsResponse
{
    public IReadOnlyList<TripReportResponse> TripReports { get; set; }
    public IReadOnlyList<FaultReportResponse> FaultReports { get; set; }
}
