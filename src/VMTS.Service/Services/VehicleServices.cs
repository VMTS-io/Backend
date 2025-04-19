using VMTS.Core.Entities;
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
    private readonly IGenericRepository<Vehicle> _repo;

    public VehicleServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repo = _unitOfWork.GetRepo<Vehicle>();
    }

    public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
    {
        if (!await IsExsistAsync<VehicleCategory>(vehicle.CategoryId))
            throw new NotFoundException("Categroy Not Found");
        if (!await IsExsistAsync<VehicleCategory>(vehicle.ModelId))
            throw new NotFoundException("Model Not Found");

        await _repo.CreateAsync(vehicle);
        await _unitOfWork.SaveChanges();

        var spec = new VehicleIncludesSpecification(vehicle.Id);
        var returnVehicle =
            await _repo.GetByIdWithSpecificationAsync(spec)
            ?? throw new NotFoundException("Vehicle Not Found");
        return returnVehicle;
    }

    public async Task<bool> DeleteVehicleAsync(string id)
    {
        var vehicle = await _repo.GetByIdAsync(id) ?? throw new Exception("Vehicle Not Found");
        try
        {
            _repo.Delete(vehicle);
            await _unitOfWork.SaveChanges();
        }
        catch
        {
            return false;
        }
        return true;
    }

    public async Task<Vehicle> GetVehicleByIdAsync(string id)
    {
        var spec = new VehicleIncludesSpecification(id);
        var vehicle =
            await _repo.GetByIdWithSpecificationAsync(spec)
            ?? throw new NotFoundException("Vehicle Not Found");
        return vehicle;
    }

    public async Task<IReadOnlyList<Vehicle>> GetAllVehiclesAsync(VehicleSpecParams specParams)
    {
        var spec = new VehicleIncludesSpecification(specParams);
        var vehicles = await _repo.GetAllWithSpecificationAsync(spec);
        return vehicles;
    }

    public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
    {
        if (!await IsExsistAsync<Vehicle>(vehicle.Id))
            throw new NotFoundException("Vehicle Not Found");
        if (!await IsExsistAsync<VehicleCategory>(vehicle.CategoryId))
            throw new NotFoundException("Categroy Not Found");
        if (!await IsExsistAsync<VehicleCategory>(vehicle.ModelId))
            throw new NotFoundException("Model Not Found");

        _repo.Update(vehicle);
        await _unitOfWork.SaveChanges();

        var spec = new VehicleIncludesSpecification(vehicle.Id);
        var returnVehicle = await _repo.GetByIdWithSpecificationAsync(spec);
        return returnVehicle!;
    }

    public async Task<bool> IsExsistAsync<T>(string id)
        where T : BaseEntity
    {
        return await _unitOfWork.GetRepo<T>().GetByIdAsync(id) is not null;
    }
}
