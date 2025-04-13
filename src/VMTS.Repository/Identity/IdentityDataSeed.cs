using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.UnitOfWork;

namespace VMTS.Repository.Identity;

public class IdentityDataSeed
{
    public static async Task SeedAsync(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
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
            var adminBusinessUser = new BusinessUser()
            {
                Id = adminUser.Id,
                Email = "bassel.admin@raafat.com",
                DisplayName = "Bassel Raafat",
                NormalizedEmail = "bassel.admin@raafat.com".ToUpper(),
                PhoneNumber = "1234",
            };
            var createResult = await userManager.CreateAsync(
                adminUser,
                configuration["Password:Admin"]
            );

            if (createResult.Succeeded)
            {
                var isRoleExists = await roleManager.RoleExistsAsync(Roles.Admin);
                if (!isRoleExists)
                    await roleManager.CreateAsync(new IdentityRole(Roles.Admin));

                await userManager.AddToRoleAsync(adminUser, Roles.Admin);

                await unitOfWork.GetRepo<BusinessUser>().CreateAsync(adminBusinessUser);
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
            var managerBusinessUser = new BusinessUser()
            {
                Id = managerUser.Id,
                Email = "bassel.manager@raafat.com",
                DisplayName = "Bassel Raafat",
                NormalizedEmail = "bassel.manager@raafat.com".ToUpper(),
                PhoneNumber = "1234",
            };

            createResult = await userManager.CreateAsync(
                managerUser,
                configuration["Password:Manager"]
            );

            if (createResult.Succeeded)
            {
                var isRoleExists = await roleManager.RoleExistsAsync(Roles.Manager);
                if (!isRoleExists)
                    await roleManager.CreateAsync(new IdentityRole(Roles.Manager));

                await userManager.AddToRoleAsync(managerUser, Roles.Manager);
                await unitOfWork.GetRepo<BusinessUser>().CreateAsync(managerBusinessUser);
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

            var mechanicBusinessUser = new BusinessUser()
            {
                Id = mechanicUser.Id,
                Email = "basell.mechainc@raafat.com",
                DisplayName = "Bassel Raafat",
                NormalizedEmail = "basell.mechainc@raafat.com",
                PhoneNumber = "1234",
            };

            createResult = await userManager.CreateAsync(
                mechanicUser,
                configuration["Password:Mechanic"]
            );

            if (createResult.Succeeded)
            {
                var isRoleExists = await roleManager.RoleExistsAsync(Roles.Mechanic);
                if (!isRoleExists)
                    await roleManager.CreateAsync(new IdentityRole(Roles.Mechanic));

                await userManager.AddToRoleAsync(mechanicUser, Roles.Mechanic);
                await unitOfWork.GetRepo<BusinessUser>().CreateAsync(mechanicBusinessUser);
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
            var driverBusinessDriver = new BusinessUser()
            {
                Id = driverUser.Id,
                Email = "basel.driver@raaft.com",
                DisplayName = "Bassel Raafat",
                NormalizedEmail = "basel.driver@raaft.com",
                PhoneNumber = "1234",
            };

            createResult = await userManager.CreateAsync(
                driverUser,
                configuration["Password:Driver"]
            );

            if (createResult.Succeeded)
            {
                var isRoleExists = await roleManager.RoleExistsAsync(Roles.Driver);
                if (!isRoleExists)
                    await roleManager.CreateAsync(new IdentityRole(Roles.Driver));
                await userManager.AddToRoleAsync(driverUser, Roles.Driver);
                await unitOfWork.GetRepo<BusinessUser>().CreateAsync(driverBusinessDriver);
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    logger.LogError("{message}", $"Error: {error.Code} - {error.Description}");
                }
            }
            await unitOfWork.SaveChanges();
        }
    }
}
