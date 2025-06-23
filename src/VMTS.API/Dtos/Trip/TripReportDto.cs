namespace VMTS.API.Dtos.Trip;

public class TripReportDto
{
    public string Id { get; set; } = default!;

    public string Duration { get; set; } = default!;

    public string Details { get; set; } = default!;

    public DateTime Date { get; set; }

    public decimal FuelCost { get; set; }
}
