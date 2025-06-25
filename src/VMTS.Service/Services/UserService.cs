using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;

namespace VMTS.Service.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppUser> _roleManager;
    private readonly IGenericRepository<BusinessUser> _businessUserRepo;

    public UserService(
        UserManager<AppUser> userManager,
        RoleManager<AppUser> roleManager,
        IGenericRepository<BusinessUser> businessUserRepo
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _businessUserRepo = businessUserRepo;
    }

    #region create

    // public Task<AppUser> CreateUserAsync(AppUser model) { }

    #endregion

    #region edit
    public async Task EditUserAsync(
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
        string role
    )
    {
        var user = await _userManager
            .Users.Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new InvalidOperationException("User not found.");

        user.FirstName = firstName;
        user.LastName = lastName;
        user.PhoneNumber = phoneNumber;
        user.NationalId = nationalId;
        user.DateOfBirth = dateOfBirth;

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
                throw new InvalidOperationException("Failed to update user role.");
        }

        var businessUser = await _businessUserRepo.GetByIdAsync(userId);
        if (businessUser != null)
        {
            businessUser.Role = role;
            _businessUserRepo.Update(businessUser); // Fix: use businessUser, not AppUser
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            throw new InvalidOperationException("Failed to update user.");
    }

    #endregion

    #region delete

    public async Task DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new ArgumentNullException("user not found");
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException("Failed to delete user.");
        var businessUser = await _businessUserRepo.GetByIdAsync(userId);
        if (businessUser != null)
        {
            _businessUserRepo.Delete(businessUser);
        }
    }

    #endregion
}
