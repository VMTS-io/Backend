using VMTS.Core.Entities.Parts;

namespace VMTS.Core.Entities.Maintenace;

public class MaintenanceFinalReportParts : BaseEntity
{
    public string MaintnenanceFinalReportId { get; set; } = default!;
    public MaintenanceFinalReport FinalReport { get; set; } = default!;
    public string PartId { get; set; } = default!;
    public Part Part { get; set; } = default!;
    public int Quantity { get; set; } = default!;
}
