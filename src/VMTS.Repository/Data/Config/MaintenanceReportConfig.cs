using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.Repository.Data.Config;

public class MaintenanceReportConfig : IEntityTypeConfiguration<MaintenaceReport>
{
    public void Configure(EntityTypeBuilder<MaintenaceReport> builder)
    {
        builder.Property(MR => MR.Cost).HasColumnType("decimal(18,2)");
    }
}
