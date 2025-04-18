using Scalar.AspNetCore;
using VMTS.API.Extensions;
using VMTS.API.Middlewares;

namespace VMTS.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddAppServices(builder.Configuration, builder.Environment);
            builder.Services.AddIdentityServices(builder.Configuration, builder.Environment);

            
            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:3000",            // Dev frontend
                            "https://veemanage.runasp.net"      // Deployment frontend
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            
            
            var app = builder.Build();

            
         

            
            await app.ApplyMigrationAsync();
            await app.ApplySeedAsync();

            app.MapOpenApi();
            app.MapScalarApiReference();

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseStaticFiles();
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
