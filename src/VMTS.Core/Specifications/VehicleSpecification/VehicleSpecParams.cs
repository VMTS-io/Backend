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

    public int PageIndex { get; set; } = 1;
    private int _pageSize = 5;
    private const int _maxSize = 10;

    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value > _maxSize ? _maxSize : value; }
    }
}
