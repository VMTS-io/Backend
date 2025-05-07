using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications.VehicleSpecification;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class VehicleServices : IVehicleSerivces
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Vehicle> _vehicleRepo;
    private readonly IGenericRepository<VehicleModel> _vehicleModelRepo;

    public VehicleServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _vehicleRepo = _unitOfWork.GetRepo<Vehicle>();
        _vehicleModelRepo = _unitOfWork.GetRepo<VehicleModel>();
    }

    #region Create
    public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
    {
        if (!_vehicleModelRepo.Exist(vehicle.ModelId))
            throw new NotFoundException("Model Not Found");

        await _vehicleRepo.CreateAsync(vehicle);
        await _unitOfWork.SaveChanges();

        var spec = new VehicleIncludesSpecification(vehicle.Id);
        var returnVehicle =
            await _vehicleRepo.GetByIdWithSpecificationAsync(spec)
            ?? throw new NotFoundException("Vehicle Not Found");
        return returnVehicle;
    }
    #endregion

    #region Delete
    public async Task<bool> DeleteVehicleAsync(string id)
    {
        var vehicle =
            await _vehicleRepo.GetByIdAsync(id) ?? throw new Exception("Vehicle Not Found");
        _vehicleRepo.Delete(vehicle);
        await _unitOfWork.SaveChanges();
        return true;
    }
    #endregion

    #region Get By Id
    public async Task<Vehicle> GetVehicleByIdAsync(string id)
    {
        var spec = new VehicleIncludesSpecification(id);
        var vehicle =
            await _vehicleRepo.GetByIdWithSpecificationAsync(spec)
            ?? throw new NotFoundException("Vehicle Not Found");
        return vehicle;
    }
    #endregion

    #region Get All
    public async Task<IReadOnlyList<Vehicle>> GetAllVehiclesAsync(VehicleSpecParams specParams)
    {
        var spec = new VehicleIncludesSpecification(specParams);
        var vehicles = await _vehicleRepo.GetAllWithSpecificationAsync(spec);
        return vehicles;
    }
    #endregion

    #region Update
    public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
    {
        if (!_vehicleRepo.Exist(vehicle.Id))
            throw new NotFoundException("Vehicle Not Found");
        if (!_vehicleModelRepo.Exist(vehicle.ModelId))
            throw new NotFoundException("Model Not Found");

        _vehicleRepo.Update(vehicle);
        await _unitOfWork.SaveChanges();

        var spec = new VehicleIncludesSpecification(vehicle.Id);
        var returnVehicle = await _vehicleRepo.GetByIdWithSpecificationAsync(spec);
        return returnVehicle!;
    }
    #endregion
}
