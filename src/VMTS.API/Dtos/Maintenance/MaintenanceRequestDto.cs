using System.ComponentModel.DataAnnotations;

namespace VMTS.API.Dtos.Maintenance;

public class MaintenanceRequestDto
{
    [Required]
    public string MechanicId { get; set; }

    [Required]
    public string VehicleId { get; set; }

    /*[Required]*/
    /*public string MaintenanceType { get; set; }*/

    public string Description { get; set; }
}
