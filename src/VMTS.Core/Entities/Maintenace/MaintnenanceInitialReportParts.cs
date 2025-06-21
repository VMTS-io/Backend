using VMTS.Core.Entities.Parts;

namespace VMTS.Core.Entities.Maintenace;

public class MaintnenanceInitialReportParts : BaseEntity
{
    public string MaintnenanceInitialReportId { get; set; } = default!;
    public MaintenanceInitialReport InitialReport { get; set; } = default!;
    public string PartId { get; set; } = default!;
    public Part Part { get; set; } = default!;
    public int Quantity { get; set; } = default!;
}
