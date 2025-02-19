using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities.Identity;

namespace VMTS.Repository.Identity;

public class IdentityDbContext : IdentityDbContext<AppUser>
{
    public IdentityDbContext (DbContextOptions<IdentityDbContext> options)
        :base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Address>().ToTable("Addresses");
    }
}