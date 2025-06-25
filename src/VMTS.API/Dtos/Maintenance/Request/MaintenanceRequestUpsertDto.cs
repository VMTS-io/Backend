namespace VMTS.API.Dtos.Maintenance.Request;

public class MaintenanceRequestUpsertDto
{
    public string MechanicId { get; set; } = default!;

    public string VehicleId { get; set; } = default!;

    public string MaintenanceCategoryId { get; set; } = default!;

    public string Description { get; set; } = default!;
}
