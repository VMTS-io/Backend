using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class VehicleCategoryServices : IVehicleCategoryServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<VehicleCategory> _repo;

    public VehicleCategoryServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repo = _unitOfWork.GetRepo<VehicleCategory>();
    }

    public async Task<VehicleCategory> CreateVehicleCategoryAsync(VehicleCategory entity)
    {
        await _repo.CreateAsync(entity);
        await _unitOfWork.SaveChanges();
        var vehicleCategory = await _repo.GetByIdAsync(entity.Id);
        return vehicleCategory is not null
            ? vehicleCategory
            : throw new NotFoundException("Vehicle Category Not Found");
    }

    public async Task<IReadOnlyList<VehicleCategory>> GetAllVehicleCategoryAsync()
    {
        var allVehicleModels = await _repo.GetAllAsync();
        return allVehicleModels;
    }

    public async Task<VehicleCategory> UpdateVehicleCategory(VehicleCategory model)
    {
        if (!_repo.Exist(model.Id))
            throw new NotFoundException("Vehicle Category Not Found");

        _repo.Update(model);
        await _unitOfWork.SaveChanges();

        var vehicleCategory =
            await _repo.GetByIdAsync(model.Id)
            ?? throw new NotFoundException("Vehicle Category Not Found");
        ;
        return vehicleCategory;
    }

    public async Task DeleteVehicleCategory(string id)
    {
        var vehicleCategory =
            await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException("Vehicle Category Not Found");
        _repo.Delete(vehicleCategory);
        await _unitOfWork.SaveChanges();
    }
}
