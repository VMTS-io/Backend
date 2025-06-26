using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Identity;

namespace VMTS.Core.Entities.Identity;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public Address Address { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public string NationalId { get; set; } = default!;

    public string PictureUrl { get; set; } = default!;
    public bool MustChangePassword { get; set; } = true;

    public string RoleId { get; set; } = default!; // ✅ FK

    public AppRole Role { get; set; } = default!; // ✅ Navigation
}
