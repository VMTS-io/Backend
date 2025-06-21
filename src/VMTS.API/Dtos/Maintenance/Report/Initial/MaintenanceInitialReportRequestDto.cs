namespace VMTS.API.Dtos.Maintenance.Report.Initial;

public class MaintenanceInitialReportRequestDto
{
    public string Notes { get; set; } = default!;
    public decimal ExpectedCost { get; set; }
    public DateOnly ExpectedFinishDate { get; set; }
    public string MaintenaceRequestId { get; set; } = default!;
    public List<MaintenanceInitialReprotPartDto> ExpectedChangedParts { get; set; } = [];
}
