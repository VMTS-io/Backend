using Microsoft.AspNetCore.Identity;
using VMTS.Core.Entities.Identity;

namespace VMTS.Core.ServicesContract;

public interface IAuthService
{
        Task<string> GenerateUniqueEmailAsync(string firstName, string lastName);
        
        private static string GenerateRandomCharacters(int length)
        {
                throw new NotImplementedException();
        }
                
        Task<string> GenerateSecurePasswordAsync(int length);
        Task<string> CreateTokenAsync(AppUser user , UserManager<AppUser> userManager);
}