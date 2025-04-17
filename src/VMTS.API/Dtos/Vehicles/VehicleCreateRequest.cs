using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleCreateRequest
{
    public string PalletNumber { get; set; }

    public DateTime JoindYear { get; set; }

    public FuelType FuelType { get; set; }

    public int KMDriven { get; set; }

    public string ModelId { get; set; }

    public string CategoryId { get; set; }
}
