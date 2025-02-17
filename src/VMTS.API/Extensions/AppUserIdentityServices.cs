using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities.Identity;
using VMTS.Repository.Identity;

namespace VMTS.API.Extensions;

public class AppUserIdentityServices
{
    public static IServiceCollection AddAppServices(IServiceCollection services, IConfiguration configuration)
    {

        services.AddIdentity<AppUser, IdentityRole>(options => { })
            .AddEntityFrameworkStores<IdentityDbContext>();



        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
        });

        return services;
    }
}
