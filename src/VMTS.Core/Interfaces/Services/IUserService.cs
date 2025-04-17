using VMTS.Core.Entities.Identity;

namespace VMTS.Core.Interfaces.Services;

public interface IUserService
{ 
   public Task<bool> EditUserAsync(
                                string userId,
                                string? UserName,
                                string? PhoneNumber,
                                string Street,
                                string Area,
                                string Governorate,
                                string Country,
                                string? Role);
}