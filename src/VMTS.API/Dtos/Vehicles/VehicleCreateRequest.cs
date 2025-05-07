using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleCreateRequest
{
    public string PalletNumber { get; set; } = default!;

    public short JoinedYear { get; set; }

    public FuelType FuelType { get; set; }

    public int KMDriven { get; set; }

    public VehicleStatus Status { get; set; }

    public string ModelId { get; set; } = default!;

    public short ModelYear { get; set; }
    // public string CategoryId { get; set; }=default!;
}
