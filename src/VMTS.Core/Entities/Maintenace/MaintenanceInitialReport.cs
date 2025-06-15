using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Maintenace;

public class MaintenanceInitialReport : BaseEntity
{
    public string Notes { get; set; } = default!;

    public decimal ExpectedCost { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public DateOnly ExpectedFinishDate { get; set; }

    public Status Status { get; set; } = Status.InProgress;

    public ICollection<MaintenaceCategory>? Category { get; set; } = [];

    public ICollection<Part>? MissingParts { get; set; } = [];

    public BusinessUser Mechanic { get; set; } = default!;
    public string MechanicId { get; set; } = default!;

    public Vehicle Vehicle { get; set; } = default!;
    public string VehicleId { get; set; } = default!;

    public MaintenaceRequest MaintenaceRequest { get; set; } = default!;
    public string MaintenaceRequestId { get; set; } = default!;
}
