using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Non_Entities_Class;

public class VehicleWithDueParts
{
    public string VehicleId { get; set; } = null!;
    public string PlateNumber { get; set; } = null!;

    public string ModelName => VehicleModel.Name;
    public string BrandName => VehicleModel.Brand;
    public string CategoryName => VehicleCategory.Name;

    public List<DuePart> DueParts { get; set; } = [];

    public bool NeedMaintenancePrediction { get; set; }

    // Additional info
    public FuelType FuelType { get; set; }
    public VehicleStatus Status { get; set; }
    public int CurrentOdometerKM { get; set; }
    public DateOnly ModelYear { get; set; }
    public string ModelId { get; set; } = default!;
    public VehicleModel VehicleModel { get; set; } = default!;
    public VehicleCategory VehicleCategory { get; set; } = default!;

    public DateTime LastAssignedDate { get; set; }
}
