using Microsoft.AspNetCore.Identity;

namespace VMTS.Core.Entities.Identity;

public class AppUser : IdentityUser 
{
    public string DisplayName { get; set; }
    
    public Address Address { get; set; }
    
    public bool MustChangePassword { get; set; } = true;
}
