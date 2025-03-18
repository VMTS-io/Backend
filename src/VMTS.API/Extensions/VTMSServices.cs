using Microsoft.EntityFrameworkCore;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.ServicesContract;
using VMTS.Repository;
using VMTS.Repository.Data;
using VMTS.Service.Services;

namespace VMTS.API.Extensions;

public static class VTMSServices
{
    public static IServiceCollection AddAppServices(IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<VTMSDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ITripRequestService, TripRequestService>();
        
        return services;
    }
}