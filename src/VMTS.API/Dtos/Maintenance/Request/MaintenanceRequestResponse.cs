using VMTS.API.Dtos.Vehicles;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Request;

public class MaintenanceRequestResponse
{
    public string Description { get; set; } = default!;

    public DateTime Date { get; set; }

    public Status Status { get; set; }

    /*public BusinessUser Manager { get; set; }*/
    /*public BusinessUser Mechanic { get; set; }*/

    public VehicleListDto Vehicle { get; set; } = default!;
}
