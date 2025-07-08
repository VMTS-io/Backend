using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Entities.Parts;

public class Part : BaseEntity
{
    public string Name { get; set; } = default!;

    public int Quantity { get; set; }

    public decimal Cost { get; set; }

    public int? LifeSpanKM { get; set; }

    public int? LifeSpanDays { get; set; }

    public bool? IsTracked { get; set; }
    public ICollection<MaintenanceTracking> MaintenancePartTrackings { get; set; } = default!;
    public ICollection<MaintenanceFinalReportParts> PartHistory { get; set; } = default!;
}
