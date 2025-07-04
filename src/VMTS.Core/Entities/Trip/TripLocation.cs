namespace VMTS.Core.Entities.Trip;

public class TripLocation
{
    public string TripId { get; set; } = default!;

    public double StartLat { get; set; }

    public double StartLng { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }

    public double DestinationLat { get; set; }

    public double DestinationLng { get; set; }

    public double Distance { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
