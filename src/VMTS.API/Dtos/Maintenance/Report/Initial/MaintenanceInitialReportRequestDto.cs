namespace VMTS.API.Dtos.Maintenance.Report.Initial;

public class MaintenanceInitialReportRequestDto
{
    public string Notes { get; set; } = default!;
    public decimal ExpectedCost { get; set; }
    public DateOnly ExpectedFinishDate { get; set; }
    public string MaintenaceRequestId { get; set; } = default!;
    public List<string> MaintenanceCategoryIds { get; set; } = [];
    public List<string>? MissingPartIds { get; set; }

    // public Status Status { get; set; } = Status.InProgress;
    // public string MechanicId { get; set; } = default!;
    // public string VehicleId { get; set; } = default!;
}
