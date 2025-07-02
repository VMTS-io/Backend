using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Maintenace;

public class MaintenanceTracking : BaseEntity
{
    public string VehicleId { get; set; } = default!;
    public string PartId { get; set; } = default!;

    public DateTime LastChangedDate { get; set; }
    public int KMAtLastChange { get; set; }

    public DateTime? NextChangeDate { get; set; }=DateTime.Now.AddDays(30);// computed
    public int? NextChangeKM { get; set; } = 19900; // computed

    public bool IsDue { get; set; } = false; // updated by a background job or on query
    public bool IsAlmostDue { get; set; } = false; // optional for pre-warning

    public Vehicle Vehicle { get; set; } = default!;
    public Part Part { get; set; } = default!;
}
