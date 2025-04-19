using VMTS.API.Errors;

namespace VMTS.API.Dtos;

public class MustChangePasswordDto
{
    public bool MustChangePassword { get; set; }

    public ApiErrorResponse ApiResponse { get; set; }
}

