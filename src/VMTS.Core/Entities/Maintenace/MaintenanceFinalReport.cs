using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Maintenace;

public class MaintenanceFinalReport : BaseEntity
{
    public string Notes { get; set; } = default!;

    public decimal TotalCost { get; set; }

    public DateTime FinishedDate { get; set; } = DateTime.UtcNow;

    // public Status Status { get; set; } = Status.Completed;

    public MaintenaceCategory MaintenanceCategories { get; set; } = default!;

    public ICollection<MaintenanceFinalReportParts> ChangedParts { get; set; } = [];

    public BusinessUser Mechanic { get; set; } = default!;
    public string MechanicId { get; set; } = default!;

    // public BusinessUser Manager { get; set; } = default!;
    // public string ManagerId { get; set; } = default!;

    public Vehicle Vehicle { get; set; } = default!;
    public string VehicleId { get; set; } = default!;

    public MaintenanceInitialReport InitialReport { get; set; } = default!;
    public string InitialReportId { get; set; } = default!;

    public MaintenaceRequest MaintenaceRequest { get; set; } = default!;
    public string MaintenaceRequestId { get; set; } = default!;
}
