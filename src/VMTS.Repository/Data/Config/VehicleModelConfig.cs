using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Repository.Data.Config;

public class VehicleModelConfig : IEntityTypeConfiguration<VehicleModel>
{
    public void Configure(EntityTypeBuilder<VehicleModel> builder)
    {
        builder
            .HasOne(vm => vm.Category)
            .WithMany(vc => vc.VehicleModels)
            .HasForeignKey(vm => vm.CategoryId);
    }
}
