using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Trip;

namespace VMTS.Repository.Data.Config;

public class RecurringTripTemplateConfig : IEntityTypeConfiguration<RecurringTripTemplate>
{
    public void Configure(EntityTypeBuilder<RecurringTripTemplate> builder)
    {
        builder
            .HasOne(t => t.Vehicle)
            .WithMany()
            .HasForeignKey(t => t.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.Driver)
            .WithMany()
            .HasForeignKey(t => t.DriverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
