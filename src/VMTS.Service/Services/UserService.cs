using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications;

namespace VMTS.Service.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IUnitOfWork _unitOfWork;

    public UserService(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;

        _unitOfWork = unitOfWork;
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
        user.NormalizedUserName = user.UserName.ToUpper();
        user.DisplayName = $"{user.FirstName} {user.LastName}";
        user.UserName = user.Email.Split('@')[0].ToLower();
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

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            throw new InvalidOperationException("Failed to update user.");

        var businessUser = await _unitOfWork.GetRepo<BusinessUser>().GetByIdAsync(userId);
        if (businessUser != null)
        {
            businessUser.DisplayName = $"{user.FirstName} {user.LastName}";
            businessUser.Email = user.Email;
            businessUser.PhoneNumber = user.PhoneNumber;
            businessUser.NormalizedEmail = user.NormalizedEmail;
            businessUser.Role = role;
            _unitOfWork.GetRepo<BusinessUser>().Update(businessUser); // Fix: use businessUser, not AppUser
        }

        await _unitOfWork.SaveChangesAsync();
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
        var businessUser = await _unitOfWork.GetRepo<BusinessUser>().GetByIdAsync(userId);
        if (businessUser != null)
        {
            _unitOfWork.GetRepo<BusinessUser>().Delete(businessUser);
        }
        await _unitOfWork.SaveChangesAsync();
    }

    #endregion


    #region  Get All
    public async Task<IReadOnlyList<BusinessUser>> GetAllUsersAsync(
        BusinessUserSpecParams specParams
    )
    {
        var spec = new BusinessUserSpecificationForDropDownList(specParams);
        return await _unitOfWork.GetRepo<BusinessUser>().GetAllWithSpecificationAsync(spec);
    }
    #endregion

    #region Get All Users based on role

    public async Task<IReadOnlyList<AppUser>> GetUsersByRoleAsync(string roleName)
    {
        // Get user IDs in the role
        var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
        var userIds = usersInRole.Select(u => u.Id).ToList();

        // Load all users with Address in one query
        return await _userManager
            .Users.Where(u => userIds.Contains(u.Id))
            .Include(u => u.Address)
            .ToListAsync();
    }

    #endregion
}
