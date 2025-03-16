using System.ComponentModel.DataAnnotations;

namespace VMTS.API.Dtos;

public class AddressDto
{
    [Required]
    public string Street { get; set; } // Example: "El Tahrir St."
    
    [Required]
    public string Area { get; set; } // Example: "Nasr City, Maadi, Mohandessin"
    
    [Required]
    public string Governorate { get; set; } // Example: "Cairo, Giza, Alexandria"
    
    [Required]
    public string Country { get; set; }  // Default value since your system is for Egypt
}