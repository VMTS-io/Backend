using System.ComponentModel.DataAnnotations;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;

namespace VMTS.API.Dtos;

public class FaultReportRequest
{ 
    [Required(ErrorMessage = "Fault type is required.")]
    public MaintenanceType FaultType { get; set; }

    [Required(ErrorMessage = "Fault address is required.")]
    [MinLength(5, ErrorMessage = "Address must be at least 5 characters.")]
    public string Address { get; set; }


    public decimal Cost{ get; set; }

    public int FuelRefile { get; set; }
    
    [Required(ErrorMessage = "Fault details are required.")]
    [MinLength(10, ErrorMessage = "Details must be at least 10 characters.")]
    public string Details { get; set; }
}