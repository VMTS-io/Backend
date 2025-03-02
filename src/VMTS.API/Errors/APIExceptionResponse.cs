namespace VMTS.API.Errors;

public class APIExceptionResponse : ApiResponse
{
    public string? ExceptionMessage { get; set; }
    
    public APIExceptionResponse(int statusCode, string? message = null , string? exceptionMessage = null) : base(statusCode, message)
    {
        ExceptionMessage = exceptionMessage;
    }
}