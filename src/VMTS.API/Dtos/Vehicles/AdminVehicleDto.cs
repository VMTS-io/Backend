namespace VMTS.API.Dtos.Vehicles;

public class AdminVehicleListDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string PalletNumber { get; set; } = default!;

    public DateOnly JoinedYear { get; set; }

    public short ModelYear { get; set; }
    // public string CategoryId { get; set; }=default!;
}
