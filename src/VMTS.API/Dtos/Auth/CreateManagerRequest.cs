using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VMTS.API.Dtos;

public class CreateManagerRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string NationalId { get; set; }
    public string PhoneNumber { get; set; }
    
    [JsonIgnore]
    [DefaultValue("Manager")]
    public string Role { get; set; } = "Manager";
    public AddressDto Address { get; set; }
}