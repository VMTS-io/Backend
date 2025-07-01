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

                    DueParts = group
                        .Where(mt => mt.Part != null) // protect against bad data
                        .Select(mt => new DuePart
                        {
                            PartId = mt.PartId,
                            PartName = mt.Part.Name ?? "Unknown",
                            IsDue = mt.IsDue,
                            IsAlmostDue = mt.IsAlmostDue,
                            LastReplacedAtKm = mt.KMAtLastChange,
                            NextChangeKm = mt.NextChangeKM,
                            CurrentKm = mt.Vehicle?.CurrentOdometerKM ?? 0,
                        })
                        .ToList(),
                };
            })
            .Where(v => v != null)
            .ToList();

        return grouped!;
    }

    #endregion
}
