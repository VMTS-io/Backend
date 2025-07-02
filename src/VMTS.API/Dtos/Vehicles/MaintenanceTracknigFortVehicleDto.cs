namespace VMTS.API.Dtos.Vehicles;

public class MaintenanceTracknigForVehicleDto
{
    public string PartId { get; set; } = default!;
    public DateTime LastChangedDate { get; set; }
    public int KMAtLastChange { get; set; }
}
