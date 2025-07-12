using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using VMTS.API.Errors;
using VMTS.Service.Exceptions;

namespace VMTS.API.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

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
        catch (InvalidOperationException ex)
        {
            await WriteErrorResponseAsync(context, 400, ex.Message);
        }
        catch (ForbbidenException ex)
        {
            await WriteErrorResponseAsync(context, 403, ex.Message);
        }
        catch (ArgumentException ex)
        {
            await WriteErrorResponseAsync(context, 400, ex.Message);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "A concurrency conflict occurred.");
            await WriteErrorResponseAsync(context, 409, ex.Message);
        }
        catch (ConflictException ex)
        {
            _logger.LogWarning(ex, "{title}", ex.Title);
            await WriteErrorResponseAsync(context, 409, ex.Message);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "A database update error occurred.");
            await WriteErrorResponseAsync(context, 500, ex.Message);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "The operation was canceled.");
            await WriteErrorResponseAsync(context, 499, ex.Message);
        }
        catch (UnprocessableEntityException ex)
        {
            _logger.LogWarning(ex, "{title}", ex.Title);
            await WriteErrorResponseAsync(context, 422, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);
            var responseBody = new ApiExceptionResponse(ex.Message, ex.StackTrace?.ToString());

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(responseBody, _jsonSerializerOptions);
            await context.Response.WriteAsync(json);
        }
    }

    private async Task WriteErrorResponseAsync(HttpContext context, int statusCode, string message)
    {
        var responseBody = new ApiErrorResponse(statusCode, message);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(responseBody, _jsonSerializerOptions);
        await context.Response.WriteAsync(json);
    }
}
