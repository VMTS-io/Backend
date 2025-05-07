using VMTS.API.Dtos.Vehicles.Model;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleListDto
{
    public string Id { get; set; } = default!;

    public string PalletNumber { get; set; } = default!;

    public DateOnly JoindYear { get; set; }

    public FuelType FuelType { get; set; }

    public int KMDriven { get; set; }

    public VehicleStatus Status { get; set; } = VehicleStatus.Active;

    public VehicleModelDto VehicleModel { get; set; } = default!;

    public DateOnly? ModelYear { get; set; }
    // public VehicleCategoryDto VehicleCategory { get; set; }
}
