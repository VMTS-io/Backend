using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.Repository.Identity.Configurations;

public class MaintenanceRequestConfiguration : IEntityTypeConfiguration<MaintenaceRequest>
{
    public void Configure(EntityTypeBuilder<MaintenaceRequest> builder)
    {
        builder.HasOne(MR => MR.Vehicle).WithMany().HasForeignKey(MR => MR.VehicleId);
    }
}
