using VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Non_Entities_Class;
using VMTS.Core.Specifications.MaintenanceTracking;

namespace VMTS.Service.Services;

public class MaintenanceTrackingService : IMaintenanceTrackingService
{
    private readonly IUnitOfWork _unitOfWork;

    public MaintenanceTrackingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #region Get Vehicles With Due Parts

    public async Task<IReadOnlyList<VehicleWithDueParts>> GetVehiclesWithDuePartsAsync(
        VehicleWithDuePartsSpecParams specParams
    )
    {
        var spec = new TrackingDuePartsSpecification(specParams);

        var trackingRecords = await _unitOfWork
            .GetRepo<MaintenanceTracking>()
            .GetAllWithSpecificationAsync(spec);

        var grouped = trackingRecords
            .Where(mt => mt.Vehicle != null) // protect against broken FK
            .GroupBy(mt => mt.VehicleId)
            .Select(group =>
            {
                var first = group.FirstOrDefault();
                var vehicle = first?.Vehicle;

                if (vehicle == null || vehicle.VehicleModel == null)
                    return null; // skip broken data

                var dueParts = group
                    .Where(mt => mt.Part != null && mt.IsAlmostDue || mt.IsDue)
                    .Select(mt => new DuePart
                    {
                        PartId = mt.PartId,
                        PartName = mt.Part.Name ?? "Unknown",
                        IsDue = mt.IsDue,
                        IsAlmostDue = mt.IsAlmostDue,
                        LastReplacedAtKm = mt.KMAtLastChange,
                        NextChangeKm = mt.NextChangeKM,
                        NextChangeDate = mt.NextChangeDate,
                        CurrentKm = mt.Vehicle?.CurrentOdometerKM ?? 0,
                    })
                    .ToList();

                bool hasNoDueParts =
                    !dueParts.Any(p => p.IsDue || p.IsAlmostDue)
                    && vehicle.NeedMaintenancePrediction == true;

                return new VehicleWithDueParts
                {
                    VehicleId = vehicle.Id ?? "N/A",
                    PlateNumber = vehicle.PalletNumber ?? "Unknown",
                    FuelType = vehicle.FuelType,
                    Status = vehicle.Status,
                    CurrentOdometerKM = vehicle.CurrentOdometerKM,
                    ModelYear = vehicle.ModelYear,
                    LastAssignedDate = vehicle.LastAssignedDate?.Date ?? DateTime.MinValue,
                    ModelId = vehicle.ModelId,
                    VehicleModel = vehicle.VehicleModel,
                    VehicleCategory =
                        vehicle.VehicleModel.Category ?? new VehicleCategory { Name = "Unknown" },
                    NeedMaintenancePrediction = vehicle.NeedMaintenancePrediction,

                    DueParts = hasNoDueParts ? new List<DuePart>() : dueParts,
                };
            })
            .Where(v => v != null)
            .ToList();

        return grouped!;
    }

    #endregion

    #region Recalculate

    public async Task RecalculateAllAsync()
    {
        var specs = new TrackingDuePartsSpecification() { Includes = [mt => mt.Vehicle] };
        var allTrackings = await _unitOfWork
            .GetRepo<MaintenanceTracking>()
            .GetAllWithSpecificationAsync(specs);
        foreach (var tracking in allTrackings)
        {
            await RecalculateRowAsync(tracking);
            _unitOfWork.GetRepo<MaintenanceTracking>().Update(tracking);
        }

        await _unitOfWork.SaveChanges();
    }

    #endregion

    #region Recalculate For Vehicle

    public async Task RecalculateForVehicleAsync(VehicleWithDuePartsSpecParams specParams)
    {
        var specs = new TrackingDuePartsSpecification(specParams);
        var allTrackings = await _unitOfWork
            .GetRepo<MaintenanceTracking>()
            .GetAllWithSpecificationAsync(specs);
        foreach (var tracking in allTrackings)
        {
            await RecalculateRowAsync(tracking);
        }
        await _unitOfWork.SaveChanges();
    }

    #endregion

    #region Recalculate Row

    public Task RecalculateRowAsync(MaintenanceTracking tracking)
    {
        var currentKM = tracking.Vehicle.CurrentOdometerKM;
        tracking.IsDue =
            (tracking.NextChangeKM > 0 && currentKM >= tracking.NextChangeKM)
            || (
                tracking.NextChangeDate.HasValue && tracking.NextChangeDate.Value <= DateTime.Today
            );

        tracking.IsAlmostDue = (
            !tracking.IsDue
            && (
                (tracking.NextChangeKM > 0 && currentKM >= tracking.NextChangeKM - 500)
                || (
                    tracking.NextChangeDate.HasValue
                    && tracking.NextChangeDate.Value <= DateTime.Today.AddDays(15)
                )
            )
        );
        return Task.CompletedTask;
    }

    #endregion
}
