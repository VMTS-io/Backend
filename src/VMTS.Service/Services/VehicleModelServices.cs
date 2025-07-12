using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications.VehicleSpecification.VehicleModelSpecifications;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class VehicleModelServices : IVehicleModelServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<VehicleModel> _modelrepo;
    private readonly IGenericRepository<VehicleCategory> _categoryrepo;

    public VehicleModelServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _modelrepo = _unitOfWork.GetRepo<VehicleModel>();
        _categoryrepo = _unitOfWork.GetRepo<VehicleCategory>();
    }

    public async Task CreateVehicleModelAsync(VehicleModel entity)
    {
        if (!await _categoryrepo.ExistAsync(entity.CategoryId))
            throw new NotFoundException("Category Not Found");
        await _modelrepo.CreateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateVehicleModelAsync(VehicleModel entity)
    {
        if (!await _categoryrepo.ExistAsync(entity.CategoryId))
            throw new NotFoundException("Category Not Found");

        if (!await _modelrepo.ExistAsync(entity.Id))
            throw new NotFoundException("Model Not Found");
        _modelrepo.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteVehicleModelAsync(string id)
    {
        var vehicleModel =
            await _modelrepo.GetByIdAsync(id) ?? throw new NotFoundException("Model Not Found");
        _modelrepo.Delete(vehicleModel);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<VehicleModel>> GetAllVehicleModelsAsync(string? categoryId)
    {
        var spec = new VehicleModelSpecification(categoryId);
        var vehicleModelList = await _modelrepo.GetAllWithSpecificationAsync(spec);
        return vehicleModelList;
    }
}
