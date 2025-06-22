namespace VMTS.API.Dtos.Maintenance.Report.Initial;

public class MaintenanceInitialReportRequestDto
{
    public string Notes { get; set; } = default!;

    // public decimal ExpectedCost { get; set; }
    public DateOnly ExpectedFinishDate { get; set; }
    public string MaintenanceRequestId { get; set; } = default!;
    public List<MaintenanceReportPartDto> ExpectedChangedParts { get; set; } = [];
}
