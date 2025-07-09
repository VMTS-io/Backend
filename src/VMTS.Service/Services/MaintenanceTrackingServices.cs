using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class MaintenanceTrackingServices : IMaintenanceTrackingServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<MaintenanceTracking> _trackingRepo;
    private readonly IGenericRepository<Vehicle> _vehicleRepo;

    public MaintenanceTrackingServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _trackingRepo = _unitOfWork.GetRepo<MaintenanceTracking>();
        _vehicleRepo = _unitOfWork.GetRepo<Vehicle>();
    }

    public async Task Create(MaintenanceTracking entity)
    {
        var vehicle =
            await _vehicleRepo.GetByIdAsync(entity.VehicleId)
            ?? throw new NotFoundException($"There is no Vehicle with id {entity.VehicleId}");
        var part =
            await _unitOfWork.GetRepo<Part>().GetByIdAsync(entity.PartId)
            ?? throw new NotFoundException($"There is no Part with id {entity.PartId}");

        entity.IsDue =
            (entity.NextChangeDate.HasValue && entity.NextChangeDate <= DateTime.Today)
            || (vehicle.CurrentOdometerKM >= entity.NextChangeKM);

        entity.IsAlmostDue =
            (entity.NextChangeDate.HasValue && entity.NextChangeDate <= DateTime.Today.AddDays(15))
            || (vehicle.CurrentOdometerKM >= entity.NextChangeKM - 500);
        entity.NextChangeKM = part.LifeSpanKM!.Value + vehicle.CurrentOdometerKM;
        entity.NextChangeDate = DateTime.Now.AddDays(part.LifeSpanDays!.Value);

        await _trackingRepo.CreateAsync(entity);
        await _unitOfWork.SaveChanges();
    }

    public async Task UpdateAll(MaintenanceTracking entity)
    {
        var existedEntity =
            await _unitOfWork.GetRepo<MaintenanceTracking>().GetByIdAsync(entity.Id)
            ?? throw new NotFoundException($"There is no Part Traking  with id {entity.Id}");
        var vehicle =
            await _vehicleRepo.GetByIdAsync(entity.VehicleId)
            ?? throw new NotFoundException($"There is no Vehicle with id {entity.VehicleId}");
        var part =
            await _unitOfWork.GetRepo<Part>().GetByIdAsync(entity.PartId)
            ?? throw new NotFoundException($"There is no Part with id {entity.PartId}");

        existedEntity.VehicleId = entity.VehicleId;
        existedEntity.PartId = entity.PartId;
        existedEntity.NextChangeKM = part.LifeSpanKM!.Value + vehicle.CurrentOdometerKM;
        entity.NextChangeDate = DateTime.Now.AddDays(part.LifeSpanDays!.Value);
        existedEntity.IsDue =
            (
                existedEntity.NextChangeDate.HasValue
                && existedEntity.NextChangeDate <= DateTime.Today
            ) || (vehicle.CurrentOdometerKM >= existedEntity.NextChangeKM);

        existedEntity.IsAlmostDue =
            (
                existedEntity.NextChangeDate.HasValue
                && existedEntity.NextChangeDate <= DateTime.Today.AddDays(15)
            ) || (vehicle.CurrentOdometerKM >= existedEntity.NextChangeKM - 500);

        _trackingRepo.Update(existedEntity);
        await _unitOfWork.SaveChanges();
    }
}
