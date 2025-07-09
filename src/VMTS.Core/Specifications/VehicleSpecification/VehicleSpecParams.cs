using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Specifications.VehicleSpecification;

public class VehicleSpecParams
{
    public string? PalletNumber { get; set; }

    public VehicleStatus? Status { get; set; }

    public string? CategoryId { get; set; }

    public int? MaxKMDriven { get; set; }

    public DateOnly? MaxJoindYear { get; set; }

    public string? ModelId { get; set; }

    public string? Sort { get; set; }

    public DateTime? TripDate { get; set; }

    public bool? AvailableForTrip { get; set; } = false;

    private string? search;

    public string? Search
    {
        get { return search; }
        set { search = value?.ToLower() ?? ""; }
    }

    public int PageIndex { get; set; } = 1;
    private int pagesize = 10;
    private const int maxsize = 50;

    public int PageSize
    {
        get { return pagesize; }
        set { pagesize = value > maxsize ? maxsize : value; }
    }
}
