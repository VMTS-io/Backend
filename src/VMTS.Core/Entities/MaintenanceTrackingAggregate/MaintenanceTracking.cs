using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Maintenace;

public class MaintenanceTracking : BaseEntity
{
    public string VehicleId { get; set; }
    public string PartId { get; set; }

    public DateTime LastChangedDate { get; set; }
    public int KMAtLastChange { get; set; }

    public DateTime? NextChangeDate { get; set; } // computed
    public int NextChangeKM { get; set; } // computed

    public bool IsDue { get; set; } // updated by a background job or on query
    public bool IsAlmostDue { get; set; } // optional for pre-warning

    public Vehicle Vehicle { get; set; }
    public Part Part { get; set; }
}
