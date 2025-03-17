using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Maintenace;

public class MaintenaceRequest : BaseEntity
{
    public string Description { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public Status Status { get; set; } = Status.Pending;

    public BusinessUser Manager { get; set; }

    public string ManagerId { get; set; }

    public BusinessUser Mechanic { get; set; }

    public string MechanicId { get; set; }

    public Vehicle Vehicle { get; set; }

    public string VehicleId { get; set; }
}
