using VMTS.API.Errors;

namespace VMTS.API.Dtos;

public class MustChangePasswordDto
{
    public bool MustChangePassword { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }


}