using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Specifications.VehicleSpecification;

public class VehicleFilterationCount : BaseSpecification<Vehicle>
{
    public VehicleFilterationCount(VehicleSpecParams specParams)
        : base(v =>
            (
                string.IsNullOrEmpty(specParams.PalletNumber)
                || v.PalletNumber == specParams.PalletNumber
            )
            && (!specParams.Status.HasValue || v.Status == specParams.Status)
            && (!specParams.MaxKMDriven.HasValue || v.KMDriven <= specParams.MaxKMDriven)
            && (specParams.MaxJoindYear.HasValue || v.JoindYear <= specParams.MaxJoindYear)
            && (
                string.IsNullOrEmpty(specParams.Search)
                || v.PalletNumber.Contains(
                    specParams.Search,
                    StringComparison.CurrentCultureIgnoreCase
                )
            )
            && (
                string.IsNullOrEmpty(specParams.CategoryId) || v.CategoryId == specParams.CategoryId
            )
            && (string.IsNullOrEmpty(specParams.ModelId) || v.ModelId == specParams.ModelId)
        ) { }
}
