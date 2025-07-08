using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;

namespace VMTS.Core.Specifications.FaultReportSepcification;

public class FaultReportSpecParams
{
    public string? DriverId { get; set; }
    public string? VehicleId { get; set; }
    public string? TripId { get; set; }
    public string? FaultType { get; set; }
    public DateTime? ReportDate { get; set; }

    private int pagesize = 10;
    private const int maxsize = 20;

    public int PageIndex { get; set; }

    public int PageSize
    {
        get { return pagesize; }
        set { pagesize = value > maxsize ? maxsize : value; }
    }

    public string? Search { get; set; }

    public string? Sort { get; set; }
}
