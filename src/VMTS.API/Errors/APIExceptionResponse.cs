namespace VMTS.API.Errors;

public class ApiExceptionResponse : ApiResponse
{
    public string? ExceptionMessage { get; set; }

    public ApiExceptionResponse(
        int statusCode,
        string? message = null,
        string? exceptionMessage = null
    )
        : base(statusCode, message)
    {
        ExceptionMessage = exceptionMessage;
    }
}

