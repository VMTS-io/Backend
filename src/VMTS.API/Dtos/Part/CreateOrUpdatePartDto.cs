namespace VMTS.API.Dtos.Part;

public class CreateOrUpdatePartDto
{
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Cost { get; set; }
}
