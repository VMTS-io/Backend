using Microsoft.AspNetCore.Identity;

namespace VMTS.Core.Entities.Identity;

public class AppRole : IdentityRole
{
    public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}
