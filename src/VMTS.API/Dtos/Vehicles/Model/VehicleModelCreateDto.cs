namespace VMTS.API.Dtos.Vehicles.Model;

public class VehicleModelCreateDto
{
    public string Name { get; set; }

    public DateTime Year { get; set; }

    public string Manufacturer { get; set; }

    public string FuelEfficiency { get; set; }
}
