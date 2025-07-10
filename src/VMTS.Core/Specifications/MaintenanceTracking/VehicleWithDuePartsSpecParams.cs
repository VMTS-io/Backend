namespace VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;

public class VehicleWithDuePartsSpecParams
{
    public string? VehicleId { get; set; }

    public string? CategoryId { get; set; }

    public string? PartId { get; set; }

    public DateTime? LastChangedDate { get; set; }

    public DateTime? NextChangeDate { get; set; } // computed

    public bool? IsDue { get; set; } // updated by a background job or on query

    public bool? IsAlmostDue { get; set; } // optional for pre-warning

    public bool? NeedMaintenancePrediction { get; set; } // optional for pre-warning

    public int PageIndex { get; set; } = 1;
    private int pagesize = 10;
    private const int maxsize = 50;

    public int PageSize
    {
        get { return pagesize; }
        set { pagesize = value > maxsize ? maxsize : value; }
    }
}
