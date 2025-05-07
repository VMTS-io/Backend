using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VMTS.API.Errors;
using VMTS.Service.Exceptions;

namespace VMTS.API.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
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
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "A concurrency conflict occurred.");
            await WriteErrorResponseAsync(
                context,
                409,
                "A concurrency conflict occurred while updating the resource."
            );
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "A database update error occurred.");
            await WriteErrorResponseAsync(
                context,
                500,
                "A database update error occurred. Please try again later."
            );
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "The operation was canceled.");
            await WriteErrorResponseAsync(context, 499, "The operation was canceled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);
            var responseBody = new ApiExceptionResponse(ex.Message, ex.StackTrace?.ToString());

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
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
