using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Report;

namespace VMTS.Repository.Data.Config;

public class FaultReportConfig : IEntityTypeConfiguration<FaultReport>
{
    public void Configure(EntityTypeBuilder<FaultReport> builder)
    {
        builder
            .HasOne(f => f.Trip)
            .WithOne(t => t.FaultReports)
            .HasForeignKey<FaultReport>(f => f.TripId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(f => f.Cost).HasColumnType("decimal(18,2)");
    }
}
