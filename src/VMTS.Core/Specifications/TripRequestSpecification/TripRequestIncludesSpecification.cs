using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Specifications.TripRequestSpecification;

public class TripRequestIncludesSpecification : BaseSpecification<TripRequest>
{
    public TripRequestIncludesSpecification(TripRequestSpecParams specParams)
        : base(tr =>
            (specParams.TripId == null || tr.Id == specParams.TripId)
            && (specParams.ManagerId == null || tr.ManagerId == specParams.ManagerId)
            && (specParams.DriverId == null || tr.DriverId == specParams.DriverId)
            && (specParams.VehicleId == null || tr.VehicleId == specParams.VehicleId)
            && (!specParams.Date.HasValue || tr.Date.Date == specParams.Date.Value.Date)
            && (!specParams.Status.HasValue || tr.Status == specParams.Status)
        )
    {
        ApplyIncludes();
    }

    private void ApplyIncludes()
    {
        Includes.Add(t => t.Vehicle);
        Includes.Add(t => t.Driver);
        Includes.Add(t => t.Vehicle.VehicleModel.Category);
        Includes.Add(t => t.Vehicle.VehicleModel.Brand);
        Includes.Add(t => t.FaultReports);
    }

    public TripRequestIncludesSpecification(
        string? managerId = null,
        string? driverId = null,
        string? vehicleId = null
    )
        : base(tr =>
            (managerId == null || tr.ManagerId == managerId)
            && (driverId == null || tr.DriverId == driverId)
            && (vehicleId == null || tr.VehicleId == vehicleId)
        ) { }
    
    public TripRequestIncludesSpecification(string id)
        :base(f => f.Id == id)
    {
        ApplyIncludes();
    }
}
