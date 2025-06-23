using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Trip;

namespace VMTS.Repository.Data.Config;

public class TripReportConfig : IEntityTypeConfiguration<TripReport>
{
    public void Configure(EntityTypeBuilder<TripReport> builder)
    {
        builder
            .HasOne(f => f.Trip)
            .WithOne(t => t.TripReports)
            .HasForeignKey<TripReport>(f => f.TripId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(f => f.FuelCost).HasColumnType("decimal(18,2)");
    }
}
