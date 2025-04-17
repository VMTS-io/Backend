namespace VMTS.API.Dtos.Trip;

public class TripReportDto
{
    public string Id { get; set; }
    public string Duration { get; set; }

    public string Details { get; set; }

    public DateTime Date { get; set; }

    public decimal FuelCost { get; set; }
}
