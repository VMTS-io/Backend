namespace VMTS.API.Dtos.TripReport;

public class TripReportListResponse
{
    public int StatusCode { get; set; }
    public List<TripReportResponse> TripReports { get; set; }
}
