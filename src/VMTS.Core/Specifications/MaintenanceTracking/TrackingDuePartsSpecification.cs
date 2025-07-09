using Microsoft.EntityFrameworkCore;
using VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;
using VMTS.Core.Entities.Parts;

namespace VMTS.Core.Specifications.MaintenanceTracking;

public class TrackingDuePartsSpecification
    : BaseSpecification<Entities.Maintenace.MaintenanceTracking>
{
    public TrackingDuePartsSpecification() { }

    public TrackingDuePartsSpecification(VehicleWithDuePartsSpecParams specParams)
        : base(mt =>
            (
                // If VehicleId is provided, match only by VehicleId (ignore IsDue/IsAlmostDue)
                (
                    !string.IsNullOrEmpty(specParams.VehicleId)
                    && mt.VehicleId == specParams.VehicleId
                )
                // If VehicleId is not provided, use IsDue or IsAlmostDue
                || (string.IsNullOrEmpty(specParams.VehicleId) && (mt.IsDue || mt.IsAlmostDue))
            )
            && (
                string.IsNullOrEmpty(specParams.CategoryId)
                || mt.Vehicle.VehicleModel.CategoryId == specParams.CategoryId
            )
            && (
                !specParams.LastChangedDate.HasValue
                || mt.LastChangedDate <= specParams.LastChangedDate
            )
            && (
                !specParams.NextChangeDate.HasValue
                || mt.NextChangeDate >= specParams.NextChangeDate
            )
            && (!specParams.IsDue.HasValue || mt.IsDue == specParams.IsDue)
            && (!specParams.IsAlmostDue.HasValue || mt.IsAlmostDue == specParams.IsAlmostDue)
        )
    {
        Includes.Add(mt => mt.Part);
        Includes.Add(mt => mt.Vehicle);
        Includes.Add(mt => mt.Vehicle.VehicleModel);
        Includes.Add(mt => mt.Vehicle.VehicleModel.Category);
        AddPaginaiton(specParams.PageIndex - 1, specParams.PageSize);
    }
}
