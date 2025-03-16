using Microsoft.AspNetCore.Identity;

namespace VMTS.Core.Entities.Identity;

public class AppUser : IdentityUser 
{
    public Address Address { get; set; }
    public bool MustChangePassword { get; set; } = true;
}
