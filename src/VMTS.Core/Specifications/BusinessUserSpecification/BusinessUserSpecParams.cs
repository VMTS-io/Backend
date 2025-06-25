namespace VMTS.Core.Specifications;

public class BusinessUserSpecParams
{
    public string Role { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? DisplayName { get; set; }

    // public bool? IsMechanic { get; set; } // Optional: if you distinguish roles by logic

    public string? Sort { get; set; }

    public string? Filter { get; set; }

    public DateTime? TripDate { get; set; }

    public DateTime? MaintenanceDate { get; set; }

    // Optional pagination support
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
