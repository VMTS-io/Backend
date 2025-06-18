using VMTS.API.Dtos.Vehicles;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Request;

public class MaintenanceRequestEdit
{
    public string Id { get; set; }

    public string Description { get; set; }

    public DateTime Date { get; set; }

    public Status Status { get; set; }

    public string MechanicId { get; set; }

    public VehicleListDto Vehicle { get; set; }
}
