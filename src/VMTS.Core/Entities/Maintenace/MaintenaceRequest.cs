using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Maintenace;

public class MaintenaceRequest : BaseEntity
{
    public string Description { get; set; }

    public DateTime Date { get; set; }

    public Status Status { get; set; }

    public BusinessUser ManagerMaintenanceRequest { get; set; }
    
    public Vehicle Vehicle { get; set; }

}