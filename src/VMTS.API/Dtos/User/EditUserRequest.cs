using VMTS.Core.Entities.Identity;

namespace VMTS.API.Dtos;

public class EditUserRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string NationalId { get; set; }
    public AddressDto? Address { get; set; }
    public string? Role { get; set; }
}
