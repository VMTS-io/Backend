using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Maintenance;

public class MaintenanceRequestEdit
{
    public string Id { get; set; }

    public string Description { get; set; }

    public DateTime Date { get; set; }

    public Status Status { get; set; }

    public string MechanicId { get; set; }

    public Vehicle Vehicle { get; set; }
}
