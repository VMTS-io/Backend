using VMTS.API.Dtos.Vehicles.Brand;
using VMTS.API.Dtos.Vehicles.Category;

namespace VMTS.API.Dtos.Vehicles.Model;

public class VehicleModelDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public BrandDto Brand { get; set; } = default!;

    public string FuelEfficiency { get; set; } = default!;

    public VehicleCategoryDto Category { get; set; } = default!;
}
