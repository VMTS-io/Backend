using VMTS.Core.Entities.Parts;

namespace VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;

public class VehicleWithDuePartsDto
{
    public string VehicleId { get; set; } = default!;
    public int CurrentOdometerKM { get; set; }
    public DateOnly ModelYear { get; set; }
    public string PlateNumber { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
    public string ModelName { get; set; } = default!;
    public string BrandName { get; set; } = default!;
    public List<DuePart> DueParts { get; set; } = default!;
    public bool NeedMaintenancePrediction { get; set; }
}
