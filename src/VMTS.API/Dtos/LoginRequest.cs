using System.ComponentModel.DataAnnotations;

namespace VMTS.API.Dtos;

public class LoginRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}