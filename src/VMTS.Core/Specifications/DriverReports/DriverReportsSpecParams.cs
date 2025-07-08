using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Specifications.DriverReports;

public class DriverReportsSpecParams
{
    public string? DriverId { get; set; }
    public string? VehicleId { get; set; }
    public string? TripId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }

    public string? FaultType { get; set; }

    private int pageSize = 10;
    private const int maxPageSize = 20;

    public int PageIndex { get; set; } = 1;

    public int PageSize
    {
        get => pageSize;
        set => pageSize = (value > maxPageSize) ? maxPageSize : value;
    }

    public string? Sort { get; set; }
    public string? Filter { get; set; }
    public string? Search { get; set; }
}
