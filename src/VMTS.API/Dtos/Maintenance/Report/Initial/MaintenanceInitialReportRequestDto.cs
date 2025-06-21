namespace VMTS.API.Dtos.Maintenance.Report.Initial;

public class MaintenanceInitialReportRequestDto
{
    public string Notes { get; set; } = default!;
    public decimal ExpectedCost { get; set; }
    public DateOnly ExpectedFinishDate { get; set; }
    public string MaintenaceRequestId { get; set; } = default!;
    public List<string> ExpectedChangedParts { get; set; } = [];
    public List<MaintenanceInitialReprotPartDto> ExpectedChangedPartsV2 { get; set; } = [];
}
