using Hangfire.Dashboard;

namespace VMTS.API.Middlewares;

public class HangfireDashboardAuthFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
   // {
       // var httpContext = context.GetHttpContext();

        // Example: Only allow logged-in users with Admin role
      //  return httpContext.User.Identity?.IsAuthenticated == true
     //       && httpContext.User.IsInRole("Admin");
    //}
}
