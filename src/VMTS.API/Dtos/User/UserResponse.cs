namespace VMTS.API.Dtos;

public class UserResponse
{ 
    public string Email { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
    public AddressDto Address { get; set; } 
    
}