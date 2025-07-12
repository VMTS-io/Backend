using VMTS.API.Dtos.Vehicles.Model;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleListDetailsDto
{
    public string Id { get; set; } = default!;

    public string PalletNumber { get; set; } = default!;

    public DateOnly JoinedYear { get; set; }

    public short ModelYear { get; set; }

    public FuelType FuelType { get; set; }

    public int CurrentOdometerKM { get; set; }

    public VehicleStatus Status { get; set; }

    public VehicleModelDto VehicleModel { get; set; } = default!;
    // public VehicleCategoryDto VehicleCategory { get; set; }
}
