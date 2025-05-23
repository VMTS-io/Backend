using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications.VehicleSpecification.VehicleModelSpecification;
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

    public async Task<VehicleModel> CreateVehicleModelAsync(VehicleModel entity)
    {
        if (!_categoryrepo.Exist(entity.CategoryId))
            throw new NotFoundException("Category Not Found");
        await _modelrepo.CreateAsync(entity);
        await _unitOfWork.SaveChanges();

        var eturnVehicleModel =
            await _modelrepo.GetByIdAsync(entity.Id)
            ?? throw new NotFoundException("Model Not Found");
        return eturnVehicleModel;
    }

    public async Task<VehicleModel> UpdateVehicleModelAsync(VehicleModel entity)
    {
        if (!_categoryrepo.Exist(entity.CategoryId))
            throw new NotFoundException("Category Not Found");

        if (!_modelrepo.Exist(entity.Id))
            throw new NotFoundException("Model Not Found");
        _modelrepo.Update(entity);
        await _unitOfWork.SaveChanges();
        var vehicleModel =
            await _modelrepo.GetByIdAsync(entity.Id)
            ?? throw new NotFoundException("Model Not Found");
        return vehicleModel;
    }

    public async Task DeleteVehicleModelAsync(string id)
    {
        var vehicleModel =
            await _modelrepo.GetByIdAsync(id) ?? throw new NotFoundException("Model Not Found");
        _modelrepo.Delete(vehicleModel);
        await _unitOfWork.SaveChanges();
    }

    public async Task<IReadOnlyList<VehicleModel>> GetAllVehicleModelsAsync()
    {
        var spec = new VehicleModelSpecification();
        var vehicleModelList = await _modelrepo.GetAllWithSpecificationAsync(spec);
        return vehicleModelList;
    }
}
