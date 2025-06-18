namespace VMTS.API.Dtos.Part;

public class PartDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Cost { get; set; }
}
