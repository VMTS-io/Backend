using Microsoft.AspNetCore.Identity;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Repository.Data;
using VMTS.Repository.Identity;

namespace VMTS.API.Extensions;

public static class SeedingExtension
{
    public static async Task ApplySeedAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<VTMSDbContext>();
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        try
        {
            await VMTSDataSeed.SeedAsync(dbContext);
            await IdentityDataSeed.SeedAsync(
                userManager,
                roleManager,
                unitOfWork,
                app.Configuration,
                loggerFactory.CreateLogger<IdentityDataSeed>()
            );
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "Erorr Happened While Updating Database");
        }
    }
}
