using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Specifications.VehicleSpecification;

public class VehicleSpecParams
{
    public string? PalletNumber { get; set; }

    public VehicleStatus? Status { get; set; }

    public int? MaxKMDriven { get; set; }

    public DateOnly? MaxJoindYear { get; set; }

    public string? ModelId { get; set; }

    public string? Sort { get; set; }

    private string? search;

    public string? Search
    {
        get { return search; }
        set { search = value?.ToLower() ?? ""; }
    }

    public int PageIndex { get; set; }
    private int pagesize = 5;
    private const int maxsize = 10;

    public int PageSize
    {
        get { return pagesize; }
        set { pagesize = value > maxsize ? maxsize : value; }
    }
}
