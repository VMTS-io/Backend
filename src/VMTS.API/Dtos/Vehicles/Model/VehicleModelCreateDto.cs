namespace VMTS.API.Dtos.Vehicles.Model;

public class VehicleModelUpsertDto
{
    public string Name { get; set; } = default!;
    public string BrandId { get; set; } = default!;
    public string CategoryId { get; set; } = default!;
}
