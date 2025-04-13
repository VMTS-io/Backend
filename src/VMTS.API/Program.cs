using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using VMTS.API.Extensions;
using VMTS.API.Middlewares;
using VMTS.Core.Entities.Identity;
using VMTS.Repository.Data;
using VMTS.Repository.Identity;

namespace VMTS.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddAppServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var dbContext = services.GetRequiredService<VTMSDbContext>();
            var identityDbContext = services.GetRequiredService<IdentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            try
            {
                await dbContext.Database.MigrateAsync();
                await VMTSDataSeed.SeedAsync(dbContext);
                await identityDbContext.Database.MigrateAsync();
                await IdentityDataSeed.SeedAsync(
                    userManager,
                    roleManager,
                    loggerFactory.CreateLogger<IdentityDataSeed>()
                );
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "Erorr Happened While Updating Database");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
