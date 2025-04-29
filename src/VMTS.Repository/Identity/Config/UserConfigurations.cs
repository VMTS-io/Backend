using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Identity;

namespace VMTS.Repository.Identity.Config;

public class UserConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasIndex(u => u.NationalId).IsUnique();

        builder.HasIndex(u => u.PhoneNumber).IsUnique();
    }
}
