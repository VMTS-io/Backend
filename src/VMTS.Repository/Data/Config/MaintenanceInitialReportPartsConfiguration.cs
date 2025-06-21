using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.Repository.Data.Config;

public class MaintenanceInitialReportPartsConfiguration
    : IEntityTypeConfiguration<MaintnenanceInitialReportParts>
{
    public void Configure(EntityTypeBuilder<MaintnenanceInitialReportParts> builder)
    {
        builder
            .HasOne(mirp => mirp.InitialReport)
            .WithMany()
            .HasForeignKey(mirp => mirp.MaintnenanceInitialReportId);
        builder.HasOne(mirp => mirp.Part).WithMany().HasForeignKey(mirp => mirp.PartId);
    }
}
