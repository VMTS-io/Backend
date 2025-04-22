using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using VMTS.API.Errors;
using VMTS.Service.Exceptions;

namespace VMTS.API.GlobalExceptionHnadler;

public class GlobalEaxceptionHandler : IExceptionHandler
{
    public ILogger<GlobalEaxceptionHandler> _logger { get; set; }
    private readonly IHostEnvironment _env;

    public GlobalEaxceptionHandler(ILogger<GlobalEaxceptionHandler> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        httpContext.Response.ContentType = "application/json";

        ApiErrorResponse responseBody;
        switch (exception)
        {
            case NotFoundException:
                responseBody = new ApiErrorResponse(
                    StatusCodes.Status404NotFound,
                    exception.Message
                );
                break;
            case BadRequestException:
                responseBody = new ApiErrorResponse(
                    StatusCodes.Status400BadRequest,
                    exception.Message
                );
                break;
            default:

                responseBody = _env.IsDevelopment()
                    ? new ApiExceptionResponse(exception.Message, exception.StackTrace?.ToString())
                    : new ApiErrorResponse(500, exception.Message);
                _logger.LogError(exception, "{message}", exception.Message);
                break;
        }
        httpContext.Response.StatusCode = responseBody.StatusCode;

        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        var opts = jsonSerializerOptions;

        var jsonRespons = JsonSerializer.Serialize(responseBody, opts);
        await httpContext.Response.WriteAsync(jsonRespons, cancellationToken);
        return true;
    }
}
