using VMTS.API.Dtos.Vehicles.Category;

namespace VMTS.API.Dtos.Vehicles.Model;

public class VehicleModelDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Manufacturer { get; set; } = default!;

    public string FuelEfficiency { get; set; } = default!;

    public VehicleCategoryDto Category { get; set; } = default!;
}
