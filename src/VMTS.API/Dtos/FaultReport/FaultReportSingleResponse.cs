using VMTS.API.Dtos.TripReport;

namespace VMTS.API.Dtos;

public class FaultReportSingleResponse
{
    public int StatusCode { get; set; }
    public FaultReportResponse FaultReport { get; set; }
}
