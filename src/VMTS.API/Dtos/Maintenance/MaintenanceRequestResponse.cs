using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Maintenance;

public class MaintenanceRequestResponse
{
    public string Description { get; set; }

    public DateTime Date { get; set; }

    public Status Status { get; set; }

    /*public BusinessUser Manager { get; set; }*/
    /*public BusinessUser Mechanic { get; set; }*/

    public Vehicle Vehicle { get; set; }
}
