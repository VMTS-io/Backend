namespace VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;

public class DuePartDto
{
    public string PartId { get; set; }
    public string PartName { get; set; }
    public string Status { get; set; } // "Due" or "AlmostDue"
    public int LastReplacedAtKm { get; set; }
    public int NextDueAtKm { get; set; }
    public int CurrentKm { get; set; }
}
