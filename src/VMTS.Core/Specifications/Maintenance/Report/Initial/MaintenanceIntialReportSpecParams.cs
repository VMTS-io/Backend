namespace VMTS.Core.Specifications.Maintenance.Report.Initial;

public class MaintenanceIntialReportSpecParams
{
    public string? MechanicId { get; set; }
    public string? VehicleId { get; set; }
    public string? MaintenanceRequestId { get; set; }
    public DateTime? Date { get; set; }
    public string? Sort { get; set; }
    public string? Search { get; set; }
    public int PageIndex { get; set; } = 1;
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
