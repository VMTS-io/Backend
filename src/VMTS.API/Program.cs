using System.Text.Json.Serialization;
using Scalar.AspNetCore;
using VMTS.API.Extensions;

namespace VMTS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            VTMSServices.AddAppServices(builder.Services, builder.Configuration);

            AppUserIdentityServices.AddAppServices(builder.Services, builder.Configuration);
                
            
            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Convert enums to strings
            });;
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
            
            app.MapOpenApi();
            app.MapScalarApiReference();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
