namespace VMTS.API.Dtos.TripReport;

public class TripReportUpdateRequest
{
    public int FuelRefile { get; set; }

    public decimal Cost { get; set; }

    public string Details { get; set; }
}
