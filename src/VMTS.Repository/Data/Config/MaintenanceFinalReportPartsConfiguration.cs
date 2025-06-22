using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.Repository.Data.Config;

public class MaintenanceFinalReportPartsConfiguration
    : IEntityTypeConfiguration<MaintenanceFinalReportParts>
{
    public void Configure(EntityTypeBuilder<MaintenanceFinalReportParts> builder)
    {
        builder
            .HasOne(mfrp => mfrp.FinalReport)
            .WithMany(mfr => mfr.ChangedParts)
            .HasForeignKey(mfrp => mfrp.MaintnenanceFinalReportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mirp => mirp.Part).WithMany().HasForeignKey(mfrp => mfrp.PartId);
    }
}
