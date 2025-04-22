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
            await WriteErrorResponseAsync(context, 404, ex.Message);
        }
        catch (BadRequestException ex)
        {
            await WriteErrorResponseAsync(context, 400, ex.Message);
        }
        catch (Exception ex)
        {
            var responseBodyV2 = new ApiExceptionResponse(ex.Message, ex.StackTrace?.ToString());

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            JsonSerializerOptions jsonSerializerOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var options = jsonSerializerOptions;

            var json = JsonSerializer.Serialize(responseBodyV2, options);
            await context.Response.WriteAsync(json);
        }
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context,
        int statusCode,
        string message
    )
    {
        var responseBody = new ApiErrorResponse(statusCode, message);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var options = jsonSerializerOptions;

        var json = JsonSerializer.Serialize(responseBody, options);
        await context.Response.WriteAsync(json);
    }
}
