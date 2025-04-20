using System.Net;
using System.Text.Json;
using VMTS.API.Errors;
using VMTS.Service.Exceptions;

namespace VMTS.API.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger _logger;
    private readonly IHostEnvironment _env;

    /*private readonly RequestDelegate _next;*/

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException ex)
        {
            await WriteErrorResponseAsync(context, 404, ex);
        }
        catch (BadRequestException ex)
        {
            await WriteErrorResponseAsync(context, 400, ex);
        }
        catch (Exception ex)
        {
            await WriteErrorResponseAsync(context, 400, ex, true);
        }
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context,
        int statusCode,
        Exception exception,
        bool isException = false
    )
    {
        var responseBody = new ApiErrorResponse(statusCode, exception.Message);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        if (isException && exception.StackTrace is not null)
            responseBody.StackTrace = exception.StackTrace;
        var options = jsonSerializerOptions;

        var json = JsonSerializer.Serialize(responseBody, options);
        await context.Response.WriteAsync(json);
    }
}
