using Microsoft.EntityFrameworkCore;
using VMTS.Repository.Data;
using VMTS.Repository.Identity;

namespace VMTS.API.Extensions;

public static class MigrationExtension
{
    public static async Task ApplyMigrationAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<VTMSDbContext>();
        var identityDbContext = services.GetRequiredService<IdentityDbContext>();
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
            await dbContext.Database.MigrateAsync();
            await identityDbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "Erorr Happened While Updating Database");
        }
    }
}
