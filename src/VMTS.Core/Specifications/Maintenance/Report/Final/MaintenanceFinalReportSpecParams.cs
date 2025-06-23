namespace VMTS.Core.Specifications.Maintenance.Report.Final;

public class MaintenanceFinalReportSpecParams
{
    public string? MechanicId { get; set; }
    public string? VehicleId { get; set; }
    public string? MaintenaceRequestId { get; set; }
    public string? InitialReportId { get; set; }

    public DateTime? FinishedDate { get; set; }

    public string? Search { get; set; }
    public string? Sort { get; set; }

    public int PageIndex { get; set; } = 1;
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
