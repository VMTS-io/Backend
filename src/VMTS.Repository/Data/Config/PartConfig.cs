using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Parts;

namespace VMTS.Repository.Data.Config;

public class PartConfig : IEntityTypeConfiguration<Part>
{
    public void Configure(EntityTypeBuilder<Part> builder)
    {
        builder.Property(P => P.Cost).HasColumnType("decimal(18,2)");

        builder
            .HasMany(p => p.MaintenancePartTrackings)
            .WithOne(m => m.Part)
            .HasForeignKey(m => m.PartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
