namespace VMTS.API.Dtos.TripReport;

public class TripReportResponse
{
    public string Id { get; set; }

    public DriverDto Driver { get; set; }

    public VehicleDto vehicle { get; set; }

    public string Details { get; set; }

    public int FuelRefile { get; set; }

    public decimal FuelCost { get; set; }

    public TripDto Trip { get; set; }

    public DateTime ReportedAt { get; set; }

    public bool Seen { get; set; }
}
