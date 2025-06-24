namespace VMTS.API.Dtos.Maintenance.Report.Final;

public class MaintenanceFinalReportUpdateDto
{
    public string Notes { get; set; } = default!;
    public List<MaintenanceReportPartDto> ChangedParts { get; set; } = [];
}
