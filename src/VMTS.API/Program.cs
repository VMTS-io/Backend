using Scalar.AspNetCore;
using VMTS.API.Extensions;
using VMTS.API.Hubs;
using VMTS.API.Middlewares;

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

            await app.ApplyMigrationAsync();
            await app.ApplySeedAsync();

            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options
                    .AddPreferredSecuritySchemes("Admin")
                    .AddHttpAuthentication(
                        "Admin",
                        auth =>
                        {
                            auth.Token = app.Configuration["Token:Admin"];
                        }
                    )
                    .AddHttpAuthentication(
                        "Manager",
                        auth =>
                        {
                            auth.Token = app.Configuration["Token:Manager"];
                        }
                    )
                    .AddHttpAuthentication(
                        "Driver",
                        auth =>
                        {
                            auth.Token = app.Configuration["Token:Driver"];
                        }
                    )
                    .AddHttpAuthentication(
                        "Mechanic",
                        auth =>
                        {
                            auth.Token = app.Configuration["Token:Mechanic"];
                        }
                    );
            });
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            // app.UseExceptionHandler();
            app.UseStaticFiles();
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.MapHub<LocationHub>("/hubs/location");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.MapControllers();
            app.Run();
        }
    }
}
