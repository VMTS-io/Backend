namespace VMTS.API.Dtos.Maintenance.Report.Initial;

public class MaintenanceInitialReportUpdateDto
{
    public string Notes { get; set; } = default!;
    public DateOnly ExpectedFinishDate { get; set; }
    public List<MaintenanceReportPartDto> ExpectedChangedParts { get; set; } = [];
}

