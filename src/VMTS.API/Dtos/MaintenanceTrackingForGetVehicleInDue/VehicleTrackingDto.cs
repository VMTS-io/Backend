using VMTS.Core.Entities.Parts;

namespace VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;

public class VehicleTrackingDto
{
    public List<DuePart> DueParts { get; set; }
    public string VehicleId { get; set; }
    public string PlateNumber { get; set; }

    public string Status { get; set; }

    public DateTime? LastChangedDate { get; set; }
    public DateTime? NextChangeDate { get; set; } = DateTime.Now;

    public int LastReplacedAtKm { get; set; }
    public int NextChangeKm { get; set; }
    public int CurrentKm { get; set; }

    public string ModelName { get; set; }
    public string BrandName { get; set; }
    public string CategoryName { get; set; }
}
