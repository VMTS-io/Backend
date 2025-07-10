using VMTS.Core.Entities.Parts;

namespace VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;

public class VehicleTrackingDto
{
    public List<DuePart> DueParts { get; set; }
    public string VehicleId { get; set; }
    public string PlateNumber { get; set; }

    public string Status { get; set; }

    public bool NeedMaintenancePrediction { get; set; }

    public string ModelName { get; set; }
    public string BrandName { get; set; }
    public string CategoryName { get; set; }
}
