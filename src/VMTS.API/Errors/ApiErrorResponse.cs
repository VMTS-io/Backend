using System.Text.Json.Serialization;

namespace VMTS.API.Errors;

public class ApiErrorResponse
{
    public int StatusCode { get; set; }

    public string Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StackTrace { get; set; }

    public ApiErrorResponse(int statusCode, string? message = null, string? stackTrace = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultErrorMessage(statusCode);
        StackTrace = stackTrace;
    }

    private static string GetDefaultErrorMessage(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Unknown Status Code",
        };
    }
}
