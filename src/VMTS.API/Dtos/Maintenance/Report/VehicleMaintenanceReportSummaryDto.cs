namespace VMTS.API.Dtos.Maintenance.Report;

public class VehicleForMaintenanceReportDto
{
    public string Id { get; set; } = default!;
    public string PalletNumber { get; set; } = default!;
    public VehicleModelForMaintenanceReportDto VehicleModel { get; set; } = default!;
}
