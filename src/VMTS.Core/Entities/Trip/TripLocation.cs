namespace VMTS.Core.Entities.Trip;

public class TripLocation
{
    public string TripId { get; set; } = default!;
    public double Lat { get; set; }
    public double Lng { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
