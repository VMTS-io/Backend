using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Maintenace;

public class MaintenanceFinalReportParts : BaseEntity
{
    public DateTime ChangedAt { get; set; }

    public int KMAtChange { get; set; }
    public int Quantity { get; set; } = default!;

    public string? VehicleId { get; set; }
    public string MaintnenanceFinalReportId { get; set; } = default!;
    public string PartId { get; set; } = default!;

    public Vehicle Vehicle { get; set; } = default!;
    public Part Part { get; set; } = default!;
    public MaintenanceFinalReport FinalReport { get; set; } = default!;
}
