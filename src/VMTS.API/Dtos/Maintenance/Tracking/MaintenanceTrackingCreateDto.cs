namespace VMTS.API.Dtos.Maintenance.Tracking;

public class MaintenanceTrackingCreateDto
{
    public string VehicleId { get; set; } = default!;
    public string PartId { get; set; } = default!;

    public DateTime LastChangedDate { get; set; }
    public int KMAtLastChange { get; set; }
}

