using System.ComponentModel.DataAnnotations;

namespace VMTS.API.Dtos;

public class Forgot_PasswordRequest
{
        [Required]
        public string Email { get; set; }
}