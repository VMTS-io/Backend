using VMTS.API.Dtos.Vehicles.Category;
using VMTS.API.Dtos.Vehicles.Model;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleListDto
{
    public string Id { get; set; }

    public string PalletNumber { get; set; }

    public DateTime JoindYear { get; set; }

    public FuelType FuelType { get; set; }

    public int KMDriven { get; set; }

    public VehicleStatus Status { get; set; } = VehicleStatus.Active;

    public VehicleModelDto VehicleModel { get; set; }

    public VehicleCategoryDto VehicleCategory { get; set; }
}
