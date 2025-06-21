using VMTS.API.Dtos.Vehicles.Category;
using VMTS.API.Dtos.Vehicles.Model;

namespace VMTS.API.Dtos;

public class VehicleDto
{
    public string Id { get; set; }
    public string PalletNumber { get; set; }
    public VehicleModelDto VehicleModelDto { get; set; }
}