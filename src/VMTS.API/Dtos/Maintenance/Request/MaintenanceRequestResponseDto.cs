using VMTS.API.Dtos.Vehicles;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Request;

public class MaintenanceRequestResponseDto
{
    public string Id { get; set; } = default!;
    public string Description { get; set; } = default!;

    public DateTime Date { get; set; }

    public Status Status { get; set; }

    public BussinessUserDto Manager { get; set; } = default!;
    public BussinessUserDto Mechanic { get; set; } = default!;

    public VehicleListDto Vehicle { get; set; } = default!;
    public string MaintenaceCategory { get; set; } = default!;
}
