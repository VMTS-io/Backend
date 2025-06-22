using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Specifications;

public class TripReportSpecParams
{
    public string? ManagerId { get; set; }
    public string? DriverId { get; set; }
    public string? TripId { get; set; }
    public string? VehicleId { get; set; }
    public TripStatus Status { get; set; }
    public DateTime? ReportDate { get; set; }

    private int pagesize = 10;
    private const int maxsize = 20;

    public int PageIndex { get; set; }

    public int PageSize
    {
        get => pagesize;
        set => pagesize = value > maxsize ? maxsize : value;
    }

    public string? Search { get; set; }
    public string? Sort { get; set; }
}
