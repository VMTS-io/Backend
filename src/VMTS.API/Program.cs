using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            builder.Services.AddAppServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);

            var app = builder.Build();

            await app.ApplyMigrationAsync();
            await app.ApplySeedAsync();

            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options
                    .AddPreferredSecuritySchemes(JwtBearerDefaults.AuthenticationScheme)
                    .AddHttpAuthentication(
                        JwtBearerDefaults.AuthenticationScheme,
                        auth =>
                        {
                            auth.Token =
                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjZlNjcwODQxLWRhODctNDc0Zi1hMGUzLTBhZWYwMGMyNjY4MSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImJhc3NlbC5hZG1pbkByYWFmYXQuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoicmFhZmF0YWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTc1MjkyMTQzNSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDM0IiwiYXVkIjoiTVlTZWN1cmVkQVBJVXNlcnMifQ.sz9waVVE_JIdMeqySSyzoARvnU7XTAarNwy68CeE8bc";
                        }
                    );
            });
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            // app.UseExceptionHandler();
            app.UseStaticFiles();
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.MapControllers();
            app.Run();
        }
    }
}
