namespace VMTS.API.Dtos;

public class UserDto
{
    public bool MustChangePassword { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }

    public int StatusCode { get; set; }

    public BussinessUserDto BussinessUserDto { get; set; }
}
