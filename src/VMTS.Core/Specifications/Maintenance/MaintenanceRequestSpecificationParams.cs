using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Specifications.Maintenance;

public class MaintenanceRequestSpecParams
{
    private int pageSize = 5;
    private const int maxPageSize = 10;
    public int PageSize
    {
        set => pageSize = value > maxPageSize ? maxPageSize : value;
        get => pageSize;
    }
    public int PageIndex { set; get; } = 1;
    public string? MechanicId { get; set; }
    public string? Id { get; set; }

    public string? VehicleId { get; set; }
    public Status? Status { get; set; }
    public DateTime? Date { get; set; }
    public string? OrderBy { get; set; }
}
