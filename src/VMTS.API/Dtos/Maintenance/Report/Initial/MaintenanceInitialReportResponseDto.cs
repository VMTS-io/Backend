namespace VMTS.API.Dtos.Maintenance.Report.Initial;

public class MaintenanceInitialReportResponseDto
{
    public string Id { get; set; } = default!;
    public string Notes { get; set; } = default!;
    public decimal ExpectedCost { get; set; }
    public DateTime Date { get; set; }
    public DateOnly ExpectedFinishDate { get; set; }

    // public Status Status { get; set; }

    public string ManagerName { get; set; } = default!;
    public string MechanicName { get; set; } = default!;
    public string VehicleName { get; set; } = default!;
    public string RequestTitle { get; set; } = default!;

    public List<string> CategoryNames { get; set; } = [];
    public List<string> MissingPartNames { get; set; } = [];
}
