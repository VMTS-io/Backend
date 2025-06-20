using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.User_Business;
using VMTS.Repository.Data;

namespace VMTS.Repository.Identity;

public class IdentityDataSeed
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true,
    };

    public static async Task SeedAsync(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        VTMSDbContext appDbContext,
        IdentityDbContext identityDbContext,
        IConfiguration configuration,
        ILogger<IdentityDataSeed> logger
    )
    {
        // Seed roles in IdentityDbContext
        var roles = new[] { "Admin", "Manager", "Mechanic", "Driver" };
        if (!await roleManager.Roles.AnyAsync())
        {
            foreach (var roleName in roles)
            {
                var role = new IdentityRole(roleName);
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    logger.LogInformation("Created role: {RoleName}", roleName);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        logger.LogError(
                            "Error creating role {RoleName}: {Code} - {Description}",
                            roleName,
                            error.Code,
                            error.Description
                        );
                    }
                }
            }
        }

        // Seed users
        if (!await userManager.Users.AnyAsync())
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "VMTS.Repository",
                configuration["SeedDataPath"] ?? "Identity/DataSeed/users.json"
            );
            if (!File.Exists(filePath))
            {
                logger.LogError("Seed file not found at {FilePath}", filePath);
                return;
            }
            var jsonText = await File.ReadAllTextAsync(filePath);
            var userSeeds = JsonSerializer.Deserialize<List<UserSeed>>(jsonText, _jsonOptions);

            if (userSeeds == null || userSeeds.Count == 0)
            {
                logger.LogWarning("No users found in {FilePath}", filePath);
                return;
            }

            var strategy = appDbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    foreach (var userSeed in userSeeds)
                    {
                        // Validate role
                        if (!roles.Contains(userSeed.Role))
                        {
                            logger.LogError(
                                "Invalid role {Role} for user {UserName}",
                                userSeed.Role,
                                userSeed.UserName
                            );
                            continue;
                        }

                        // Validate email uniqueness
                        if (await userManager.FindByEmailAsync(userSeed.Email) != null)
                        {
                            logger.LogWarning(
                                "User with email {Email} already exists",
                                userSeed.Email
                            );
                            continue;
                        }

                        // Create AppUser
                        var appUser = new AppUser
                        {
                            Id = userSeed.Id,
                            Email = userSeed.Email,
                            UserName = userSeed.UserName,
                            NormalizedEmail = userSeed.Email.ToUpper(),
                            PhoneNumber = userSeed.PhoneNumber,
                            FirstName = userSeed.FirstName,
                            LastName = userSeed.LastName,
                            DisplayName = userSeed.DisplayName,
                            Address = new Address
                            {
                                Street = userSeed.Address.Street,
                                Area = userSeed.Address.Area,
                                Governorate = userSeed.Address.Governorate,
                                Country = userSeed.Address.Country,
                            },
                            DateOfBirth = DateOnly.Parse(userSeed.DateOfBirth),
                            NationalId = userSeed.NationalId,
                            PictureUrl = userSeed.PictureUrl,
                            MustChangePassword = userSeed.MustChangePassword,
                        };

                        var password = configuration[$"Password:{userSeed.Role}"] ?? "P@ssw0rd123!";
                        var createResult = await userManager.CreateAsync(appUser, password);

                        if (!createResult.Succeeded)
                        {
                            foreach (var error in createResult.Errors)
                            {
                                logger.LogError(
                                    "Error creating user {UserName}: {Code} - {Description}",
                                    appUser.UserName,
                                    error.Code,
                                    error.Description
                                );
                            }
                            continue;
                        }

                        // Set Address.AppUserId and save Address
                        // appUser.Address.AppUserId = appUser.Id;
                        // await identityDbContext.AddAsync(appUser.Address);
                        // await identityDbContext.SaveChangesAsync();

                        // Assign role
                        var roleResult = await userManager.AddToRoleAsync(appUser, userSeed.Role);
                        if (!roleResult.Succeeded)
                        {
                            foreach (var error in roleResult.Errors)
                            {
                                logger.LogError(
                                    "Error assigning role {Role} to user {UserName}: {Code} - {Description}",
                                    userSeed.Role,
                                    appUser.UserName,
                                    error.Code,
                                    error.Description
                                );
                            }
                            await userManager.DeleteAsync(appUser);
                            continue;
                        }

                        // Create BusinessUser
                        var businessUser = new BusinessUser
                        {
                            Id = appUser.Id,
                            Email = userSeed.Email,
                            NormalizedEmail = userSeed.Email.ToUpper(),
                            PhoneNumber = userSeed.PhoneNumber,
                            DisplayName = userSeed.DisplayName,
                            Role = userSeed.Role,
                        };

                        await appDbContext.AddAsync(businessUser);
                        logger.LogInformation(
                            "Created user {UserName} with role {Role}",
                            appUser.UserName,
                            userSeed.Role
                        );
                    }

                    await appDbContext.SaveChangesAsync();
                    logger.LogInformation("Seeded all users successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Seeding users failed");
                    foreach (var userSeed in userSeeds)
                    {
                        var existingUser = await userManager.FindByNameAsync(userSeed.UserName);
                        if (existingUser != null)
                        {
                            await userManager.DeleteAsync(existingUser);
                        }
                    }
                    throw;
                }
            });
        }
    }

    private class UserSeed
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public AddressSeed Address { get; set; } = default!;
        public string DateOfBirth { get; set; } = default!;
        public string NationalId { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public bool MustChangePassword { get; set; }
        public string Role { get; set; } = default!;
    }

    private class AddressSeed
    {
        public string Street { get; set; } = default!;
        public string Area { get; set; } = default!;
        public string Governorate { get; set; } = default!;
        public string Country { get; set; } = default!;
    }
}
