namespace VMTS.API.Dtos.Vehicles;

public class VehicleWithHistoryDto
{
    public VehicleUpsertDto Vehicle { get; set; } = default!;
    public List<MaintenanceTracknigForVehicleDto> MaintenanceHistory { get; set; } = default!;
}
