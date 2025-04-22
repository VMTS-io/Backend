using System.Text.Json.Serialization;

namespace VMTS.API.Errors;

public class ApiExceptionResponse : ApiErrorResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StackTracer { get; set; }

    public ApiExceptionResponse(string message, string? stackTracer = null)
        : base(500, message)
    {
        StackTracer = stackTracer;
    }
}
