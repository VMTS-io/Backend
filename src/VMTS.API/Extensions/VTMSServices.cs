using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VMTS.API.Errors;
using VMTS.API.Helpers;
using VMTS.API.Middlewares;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Repository;
using VMTS.Repository.Data;
using VMTS.Service.Services;

namespace VMTS.API.Extensions;

public static class VTMSServices
{
    public static IServiceCollection AddAppServices(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<ApiBehaviorOptions>(options =>
            options.InvalidModelStateResponseFactory = (actionContext) =>
            {
                var errors = actionContext
                    .ModelState.Where(M => M.Value?.Errors.Count > 0)
                    .ToDictionary(
                        m => m.Key,
                        m => m.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                    );
                var response = new ApiErrorValidationResponse() { Errors = errors };
                return new BadRequestObjectResult(response);
            }
        );
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Convert enums to strings
            });
        services.AddOpenApi();
        services.AddSingleton<ExceptionMiddleware>();
        services.AddDbContext<VTMSDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IMaintenanceRequestServices, MaintenanceRequestServices>();
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }
}
