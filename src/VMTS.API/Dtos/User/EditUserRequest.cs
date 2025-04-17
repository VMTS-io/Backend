using VMTS.Core.Entities.Identity;

namespace VMTS.API.Dtos;

public class EditUserRequest
{
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public AddressDto? Address { get; set; }
    public string? Role { get; set; }
}
