using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications.Maintenance.Tracking;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class MaintenanceTrackingServices : IMaintenanceTrackingServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<MaintenanceTracking> _trackingRepo;

    public MaintenanceTrackingServices(
        IUnitOfWork unitOfWork,
        IGenericRepository<MaintenanceTracking> trackingRepo
    )
    {
        _unitOfWork = unitOfWork;
        _trackingRepo = _unitOfWork.GetRepo<MaintenanceTracking>();
    }

    public async Task Create(MaintenanceTracking entity)
    {
        var vehicle =
            await _unitOfWork.GetRepo<Vehicle>().GetByIdAsync(entity.VehicleId)
            ?? throw new NotFoundException($"There is no Vehicle with id {entity.VehicleId}");
        var part =
            await _unitOfWork.GetRepo<Part>().GetByIdAsync(entity.PartId)
            ?? throw new NotFoundException($"There is no Part with id {entity.PartId}");

        entity.IsDue =
            (entity.NextChangeDate.HasValue && entity.NextChangeDate <= DateTime.Today)
            || (entity.NextChangeKM.HasValue && vehicle.CurrentOdometerKM >= entity.NextChangeKM);

        entity.IsAlmostDue =
            (entity.NextChangeDate.HasValue && entity.NextChangeDate <= DateTime.Today.AddDays(15))
            || (
                entity.NextChangeKM.HasValue
                && vehicle.CurrentOdometerKM >= entity.NextChangeKM - 500
            );
        entity.NextChangeKM = part.LifeSpanKM + vehicle.CurrentOdometerKM;
        // entity.NextChangeDate = DateTime.UtcNow.AddDays(part.LifeSpanDays!.Value);
        await _unitOfWork.GetRepo<MaintenanceTracking>().CreateAsync(entity);
        await _unitOfWork.SaveChanges();
    }

    public async Task UpdateAll(MaintenanceTracking entity)
    {
        var existedEntity =
            await _unitOfWork.GetRepo<MaintenanceTracking>().GetByIdAsync(entity.Id)
            ?? throw new NotFoundException($"There is no Part Traking  with id {entity.Id}");
        var vehicle =
            await _unitOfWork.GetRepo<Vehicle>().GetByIdAsync(entity.VehicleId)
            ?? throw new NotFoundException($"There is no Vehicle with id {entity.VehicleId}");
        var part =
            await _unitOfWork.GetRepo<Part>().GetByIdAsync(entity.PartId)
            ?? throw new NotFoundException($"There is no Part with id {entity.PartId}");

        existedEntity.VehicleId = entity.VehicleId;
        existedEntity.PartId = entity.PartId;
        existedEntity.NextChangeKM = part.LifeSpanKM + vehicle.CurrentOdometerKM;
        // existedEntity.NextChangeDate = DateTime.UtcNow.AddDays(part.LifeSpanDays!.Value);
        #region propably will replaced with omar's methods
        existedEntity.IsDue =
            (
                existedEntity.NextChangeDate.HasValue
                && existedEntity.NextChangeDate <= DateTime.Today
            )
            || (
                existedEntity.NextChangeKM.HasValue
                && vehicle.CurrentOdometerKM >= existedEntity.NextChangeKM
            );

        existedEntity.IsAlmostDue =
            (
                existedEntity.NextChangeDate.HasValue
                && existedEntity.NextChangeDate <= DateTime.Today.AddDays(15)
            )
            || (
                existedEntity.NextChangeKM.HasValue
                && vehicle.CurrentOdometerKM >= existedEntity.NextChangeKM - 500
            );

        #endregion
        _unitOfWork.GetRepo<MaintenanceTracking>().Update(existedEntity);
        await _unitOfWork.SaveChanges();
    }

    public async Task GetVehiclePatsHistory(string vehicleId)
    {
        var specs = new MaintenanceTrackingSpecification()
        {
            Criteria = mt => mt.VehicleId == vehicleId,
            Includes = [mt => mt.Part, mt => mt.Vehicle],
        };
        var tracking = await _unitOfWork
            .GetRepo<MaintenanceTracking>()
            .GetAllWithSpecificationAsync(specs);
        // var obj = new {
        //     Vehicle = tracking.
        //
        // };
    }
}
