using Hangfire;
using OfficeOpenXml;
using Scalar.AspNetCore;
using VMTS.API.Extensions;
using VMTS.API.Hubs;
using VMTS.API.Middlewares;
using VMTS.Repository.Data.Jobs;
using VMTS.Service.Jobs;

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
            ExcelPackage.License.SetNonCommercialPersonal("Bassel Raafat");
            var app = builder.Build();

            await using (var scope = app.Services.CreateAsyncScope())
            {
                var recalculatejob = scope.ServiceProvider.GetRequiredService<RecalculateJob>();

                var recurringJobManager =
                    scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                recurringJobManager.AddOrUpdate(
                    "RecalculateAllJob",
                    () => recalculatejob.RunRecalculateAll(),
                    Cron.Daily(21, 30)
                );

                var assignDailyTrip = scope.ServiceProvider.GetRequiredService<AssignDailyTrip>();
                recurringJobManager.AddOrUpdate(
                    "GenerateDailyTripsJob",
                    () => assignDailyTrip.RunAssignDailyTrip(),
                    Cron.Daily(21, 30)
                );

                var UpdateNextMaintenanceDate =
                    scope.ServiceProvider.GetRequiredService<UpdateNextMaintenanceDateJob>();
                recurringJobManager.AddOrUpdate(
                    "UpdateNextMaintenanceDate",
                    () => UpdateNextMaintenanceDate.SetNextMaintenanceDate(),
                    Cron.Weekly()
                );
            }

            // await app.ApplyMigrationAsync();
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
            app.UseHangfireDashboard(
                "/hangfire",
                new DashboardOptions { Authorization = [new HangfireDashboardAuthFilter()] }
            );

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
