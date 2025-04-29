using VMTS.Core.Entities.Identity;

namespace VMTS.Core.Interfaces.Services;

public interface IUserService
{
    public Task<bool> EditUserAsync(
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
        string? role
    );
}
