using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Specifications;

namespace VMTS.Core.Interfaces.Services;

public interface IUserService
{
    // Task<AppUser> CreateUserAsync(AppUser user);

    Task EditUserAsync(
        string userId,
        string firstName,
        string lastName,
        string nationalId,
        DateOnly dateOfBirth,
        string PhoneNumber,
        string street,
        string area,
        string governorate,
        string country,
        string role
    );

    Task DeleteUserAsync(string userId);

    Task<IReadOnlyList<BusinessUser>> GetAllUsersAsync(BusinessUserSpecParams specParams);

    Task<IReadOnlyList<AppUser>> GetUsersByRoleAsync(string roleName);
}
