namespace VMTS.Core.Entities.Trip;

public class TripReport : BaseEntity
{
    public string Duration { get; set; }

    public string Details { get; set; }

    public DateTime Date { get; set; }

    public decimal FuelCost { get; set; }
}