namespace VMTS.API.Dtos.Maintenance.Report;

public class VehicleModelForMaintenanceReportDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string FuelEfficiency { get; set; } = default!;

    public string Brand { get; set; } = default!;

    public string Category { get; set; } = default!;
}
