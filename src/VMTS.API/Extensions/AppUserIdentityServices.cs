using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VMTS.Core.Entities.Identity;
using VMTS.Core.ServicesContract;
using VMTS.Repository.Identity;
using VMTS.Service.Services;

namespace VMTS.API.Extensions;

public static class AppUserIdentityServices
{
    public static IServiceCollection AddAppServices(IServiceCollection services, IConfiguration configuration)
    {

        services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;  // Email must be unique
                options.User.AllowedUserNameCharacters = null; // Allow any username format
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();;

        services.AddScoped(typeof(IAuthService), typeof(AuthService));

        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
        });
        
        
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => 
            options.TokenValidationParameters = new TokenValidationParameters()
            {   
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:ValidIssuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:secretkey"])),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:Expires"]))
            }
            );
        
        return services;
    }
}
