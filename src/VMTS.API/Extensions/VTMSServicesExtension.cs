using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VMTS.API.Controllers;
using VMTS.API.Errors;
using VMTS.API.GlobalExceptionHnadler;
using VMTS.API.Helpers;
using VMTS.API.Middlewares;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.ServicesContract;
using VMTS.Repository;
using VMTS.Repository.Data;
using VMTS.Repository.Repositories;
using VMTS.Service.Services;

namespace VMTS.API.Extensions;

public static class VTMSServicesExtension
{
    public static IServiceCollection AddAppServices(
        this IServiceCollection services,
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
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalEaxceptionHandler>();
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Convert enums to strings
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()); // Custom DateOnly converter

                options.JsonSerializerOptions.MaxDepth = 128;
            });
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddCors();
        services.AddOpenApi();

        services.AddSingleton<ExceptionMiddleware>();
        services.AddDbContext<VTMSDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultDeploymentConnection"),
                options =>
                    options.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null
                    )
            );
        });

        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ITripRequestService, TripRequestService>();
        services.AddScoped<IMaintenanceRequestServices, MaintenanceRequestServices>();
        services.AddScoped<IVehicleSerivces, VehicleServices>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVehicleModelServices, VehicleModelServices>();
        services.AddScoped<IVehicleCategoryServices, VehicleCategoryServices>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


        return services;
    }
}
