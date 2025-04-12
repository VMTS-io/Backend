using System.Net;
using System.Text.Json;
using VMTS.API.Errors;

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var responsBody = _env.IsDevelopment()
                ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace?.ToString())
                : new ApiExceptionResponse(500, ex.Message);

            JsonSerializerOptions jsonSerializerOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var opts = jsonSerializerOptions;

            var jsonRespons = JsonSerializer.Serialize(responsBody, opts);
            await context.Response.WriteAsync(jsonRespons);
        }
    }
}
