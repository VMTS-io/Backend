using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Maintenace;

public class MaintenaceRequest : BaseEntity
{
    public string Description { get; set; } = default!;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public Status Status { get; set; } = Status.Pending;

    public BusinessUser Manager { get; set; } = default!;

    public string ManagerId { get; set; } = default!;

    public BusinessUser Mechanic { get; set; } = default!;

    public string MechanicId { get; set; } = default!;

    public Vehicle Vehicle { get; set; } = default!;

    public string VehicleId { get; set; } = default!;

    public MaintenaceCategory MaintenanceCategory { get; set; } = default!;
    public string MaintenanceCategoryId { get; set; } = default!;

    public MaintenanceInitialReport InitialReport { get; set; } = default!;
    public MaintenanceFinalReport FinalReport { get; set; } = default!;

    // public FaultReport FaultReport { get; set; } = default!;
    //
    // public string? FaultReportId { get; set; } = default!;
}
