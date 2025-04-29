using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Identity;

namespace VMTS.Core.Entities.Identity;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public Address Address { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string NationalId { get; set; }

    public string PictureUrl { get; set; }
    public bool MustChangePassword { get; set; } = true;
}
