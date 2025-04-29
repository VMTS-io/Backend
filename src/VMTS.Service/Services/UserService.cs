using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Interfaces.Services;

namespace VMTS.Service.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> EditUserAsync(
        string userId,
        string firstName,
        string lastName,
        string nationalId,
        DateOnly dateOfBirth,
        string phoneNumber,
        string street,
        string area,
        string governorate,
        string country,
        string? role
    )
    {
        var user = await _userManager
            .Users.Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return false;

        user.FirstName = firstName;
        user.LastName = lastName;
        user.PhoneNumber = phoneNumber;
        user.NationalId = nationalId;
        user.DateOfBirth = dateOfBirth;
        // Address update
        user.Address.Street = street;
        user.Address.Area = area;
        user.Address.Governorate = governorate;
        user.Address.Country = country;

        // Role update
        if (!string.IsNullOrWhiteSpace(role))
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
                return false;
        }

        var updateResult = await _userManager.UpdateAsync(user);
        return updateResult.Succeeded;
    }
}
