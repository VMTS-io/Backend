using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Trip;

namespace VMTS.Repository.Data.Config;

public class TripReportConfig : IEntityTypeConfiguration<TripReport>
{
    public void Configure(EntityTypeBuilder<TripReport> builder)
    {
        builder.Property(TR => TR.FuelCost).HasColumnType("decimal(18,2)");
    }
}
