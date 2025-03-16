using Microsoft.AspNetCore.Identity;
using VMTS.Core.Entities.Identity;

namespace VMTS.Core.ServicesContract;

public interface IAuthService
{
        Task<string> GenerateUniqueEmailAsync(string firstName, string lastName);
        
       
        
        Task<string> CreateTokenAsync(AppUser user , UserManager<AppUser> userManager);
}