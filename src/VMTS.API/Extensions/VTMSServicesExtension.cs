using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using VMTS.API.ActionFilters;
using VMTS.API.Errors;
using VMTS.API.GlobalExceptionHnadler;
using VMTS.API.Helpers;
using VMTS.API.Hubs;
using VMTS.API.Middlewares;
using VMTS.Core.Entities.Identity;
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
        // services.Configure<ApiBehaviorOptions>(options =>
        //     options.InvalidModelStateResponseFactory = (actionContext) =>
        //     {
        //         var errors = actionContext
        //             .ModelState.Where(M => M.Value?.Errors.Count > 0)
        //             .ToDictionary(
        //                 m => m.Key ?? "_glabal",
        //                 m => m.Value!.Errors.Select(e => e.ErrorMessage).ToList()
        //             );
        //         var response = new ApiErrorValidationResponse() { Errors = errors };
        //         return new BadRequestObjectResult(response);
        //     }
        // );

        // services.Configure<ApiBehaviorOptions>(options =>
        // {
        //     options.InvalidModelStateResponseFactory = context =>
        //     {
        //         var modelState = context.ModelState;
        //
        //         var problemDetails = new ValidationProblemDetails(modelState)
        //         {
        //             Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        //             Title = "Validation failed",
        //             Status = StatusCodes.Status400BadRequest,
        //             Instance = context.HttpContext.Request.Path,
        //         };
        //
        //         return new BadRequestObjectResult(problemDetails)
        //         {
        //             ContentTypes = { "application/problem+json" },
        //         };
        //     };
        // });

        services.AddSignalR();

        var redisConfiguration = new ConfigurationOptions
        {
            EndPoints = { "humane-imp-25093.upstash.io:6379" },
            User = "default",
            Password = "AWIFAAIjcDE5OWE1YmZlYzYwMGU0MGE3YWRhNTFlZTk3ZTkxYjEyNXAxMA",
            Ssl = true,
            AbortOnConnectFail = false,
        };

        // ✅ Register for caching
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = redisConfiguration;
        });

        // ✅ Register for IConnectionMultiplexer (used by TripLocationService)
        var multiplexer = ConnectionMultiplexer.Connect(redisConfiguration);
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        services.AddScoped<ILocationBroadcaster, SignalRLocationBroadcaster>();
        services.AddScoped<ITripLocationService, TripLocationService>();
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
        services.AddScoped(typeof(ValidateModelActionFilter<>));
        services.AddCors();
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

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

        services.AddScoped<ITripReportService, TripReportService>();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IFaultReportService, FaultReportService>();
        services.AddScoped<ITripRequestService, TripRequestService>();
        services.AddScoped<IDriverReportsService, DriverReportsService>();
        services.AddScoped<IMechanicReportsServices, MechaincReportsServices>();
        services.AddScoped<IMaintenanceRequestServices, MaintenanceRequestServices>();
        services.AddScoped<IVehicleSerivces, VehicleServices>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVehicleModelServices, VehicleModelServices>();
        services.AddScoped<IVehicleCategoryServices, VehicleCategoryServices>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IMaintenanceInitialReportServices, MaintenanceInitialReportServices>();
        services.AddScoped<IMaintenanceFinalReportServices, MaintenanceFinalReportServices>();
        services.AddScoped<IPartService, PartService>();
        services.AddScoped<IMaintenanceCategoryServices, MaintenanceCategoryServices>();
        services.AddScoped<IMaintenanceTrackingServices, MaintenanceTrackingServices>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}
