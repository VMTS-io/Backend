using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.Repository.Data.Config;

public class MaintenanceInitialReportPartsConfiguration
    : IEntityTypeConfiguration<MaintenanceInitialReportParts>
{
    public void Configure(EntityTypeBuilder<MaintenanceInitialReportParts> builder)
    {
        builder
            .HasOne(mirp => mirp.InitialReport)
            .WithMany(mir => mir.ExpectedChangedParts)
            .HasForeignKey(mirp => mirp.MaintnenanceInitialReportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mirp => mirp.Part).WithMany().HasForeignKey(mirp => mirp.PartId);
    }
}
