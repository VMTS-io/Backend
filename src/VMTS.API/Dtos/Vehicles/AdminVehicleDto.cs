namespace VMTS.API.Dtos.Vehicles;

public class AdminVehicleListDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string PalletNumber { get; set; } = default!;

    public DateOnly JoindYear { get; set; }

    public DateOnly? ModelYear { get; set; }
    // public string CategoryId { get; set; }=default!;
}
