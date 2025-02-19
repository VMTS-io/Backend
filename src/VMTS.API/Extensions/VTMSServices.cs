using Microsoft.EntityFrameworkCore;
using VMTS.Repository.Data;

namespace VMTS.API.Extensions;

public static class VTMSServices
{
    public static IServiceCollection AddAppServices(IServiceCollection services, IConfiguration configuration)
    {


        services.AddDbContext<VTMSDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        
        return services;
    }
}