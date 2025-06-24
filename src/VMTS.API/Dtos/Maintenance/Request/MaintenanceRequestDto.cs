using System.ComponentModel.DataAnnotations;

namespace VMTS.API.Dtos.Maintenance.Request;

public class MaintenanceRequestDto
{
    [Required]
    public string MechanicId { get; set; } = default!;

    [Required]
    public string VehicleId { get; set; } = default!;

    /*[Required]*/
    /*public string MaintenanceType { get; set; }*/

    public string Description { get; set; } = default!;
}
