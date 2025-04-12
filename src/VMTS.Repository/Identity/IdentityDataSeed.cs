using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Helpers;

namespace VMTS.Repository.Identity;

public class IdentityDataSeed
{
    public static async Task SeedAsync(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<IdentityDataSeed> logger
    )
    {
        if (!roleManager.Roles.Any())
        {
            var roles = new List<IdentityRole>
            {
                new(Roles.Admin),
                new(Roles.Manager),
                new(Roles.Mechanic),
                new(Roles.Driver),
            };
            foreach (var role in roles)
                await roleManager.CreateAsync(role);
        }
        if (!userManager.Users.Any())
        {
            var adminUser = new AppUser()
            {
                Email = "bassel.admin@raafat.com",
                UserName = "raafatadmin",
            };

            var createResult = await userManager.CreateAsync(adminUser, "P@33word");

            if (createResult.Succeeded)
            {
                var isRoleExists = await roleManager.RoleExistsAsync(Roles.Admin);
                if (!isRoleExists)
                    await roleManager.CreateAsync(new IdentityRole(Roles.Admin));

                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    logger.LogError("{message}", $"Error: {error.Code} - {error.Description}");
                }
            }
            var managerUser = new AppUser()
            {
                Email = "bassel.manager@raafat.com",
                UserName = "raafatmanager",
            };

            createResult = await userManager.CreateAsync(managerUser, "P@33word");

            if (createResult.Succeeded)
            {
                var isRoleExists = await roleManager.RoleExistsAsync(Roles.Manager);
                if (!isRoleExists)
                    await roleManager.CreateAsync(new IdentityRole(Roles.Manager));

                await userManager.AddToRoleAsync(managerUser, Roles.Manager);
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    logger.LogError("{message}", $"Error: {error.Code} - {error.Description}");
                }
            }
            var mechanicUser = new AppUser()
            {
                Email = "basell.mechainc@raafat.com",
                UserName = "raafatmechanic",
            };

            createResult = await userManager.CreateAsync(mechanicUser, "P@33word");

            if (createResult.Succeeded)
            {
                var isRoleExists = await roleManager.RoleExistsAsync(Roles.Mechanic);
                if (!isRoleExists)
                    await roleManager.CreateAsync(new IdentityRole(Roles.Mechanic));

                await userManager.AddToRoleAsync(mechanicUser, Roles.Mechanic);
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    logger.LogError("{message}", $"Error: {error.Code} - {error.Description}");
                }
            }

            var driverUser = new AppUser()
            {
                Email = "basel.driver@raaft.com",
                UserName = "raaftdriver",
            };

            createResult = await userManager.CreateAsync(driverUser, "P@33word");

            if (createResult.Succeeded)
            {
                var isRoleExists = await roleManager.RoleExistsAsync(Roles.Driver);
                if (!isRoleExists)
                    await roleManager.CreateAsync(new IdentityRole(Roles.Driver));
                await userManager.AddToRoleAsync(driverUser, Roles.Driver);
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    logger.LogError("{message}", $"Error: {error.Code} - {error.Description}");
                }
            }
        }
    }
}
