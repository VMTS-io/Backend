using VMTS.Core.Entities.Parts;

namespace VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;

public class VehicleWithDuePartsDto
{
    public string VehicleId { get; set; }
    public string PlateNumber { get; set; }
    public List<DuePart> DueParts { get; set; }
}

