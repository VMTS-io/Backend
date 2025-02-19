using Microsoft.EntityFrameworkCore;

namespace VMTS.Core.Entities.Maintenace;

public class MaintenaceTracking : BaseEntity
{
    public DateTime LastTimeMaintained { get; set; }

    public int Count { get; set; }

    public DateTime NextMaintainDate { get; set; }

    public Type Type { get; set; }
}