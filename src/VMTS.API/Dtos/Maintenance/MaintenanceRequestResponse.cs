using VMTS.API.Dtos.Vehicles;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance;

public class MaintenanceRequestResponse
{
    public string Description { get; set; }

    public DateTime Date { get; set; }

    public Status Status { get; set; }

    /*public BusinessUser Manager { get; set; }*/
    /*public BusinessUser Mechanic { get; set; }*/

    public VehicleListDto Vehicle { get; set; }
}
