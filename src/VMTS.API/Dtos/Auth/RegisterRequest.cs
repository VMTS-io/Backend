namespace VMTS.API.Dtos;

public class RegisterRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
    public AddressDto Address { get; set; }
    
}