using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities.Identity;

namespace VMTS.Repository.Identity;

public class IdentityDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>().ToTable("AspNetUsers");
        builder.Entity<AppRole>().ToTable("AspNetRoles");

        builder.Entity<Address>().ToTable("Addresses");
        builder.Entity<AppUser>().HasIndex(u => u.UserName).IsUnique(false);

        builder
            .Entity<AppUser>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
