using VMTS.Core.Entities.Maintenace;
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
    private readonly IGenericRepository<MaintenanceTracking> _trackingRepo;
    private readonly IPartService _partService;

    public VehicleServices(IUnitOfWork unitOfWork, IPartService partService)
    {
        _unitOfWork = unitOfWork;
        _vehicleRepo = _unitOfWork.GetRepo<Vehicle>();
        _vehicleModelRepo = _unitOfWork.GetRepo<VehicleModel>();
        _trackingRepo = _unitOfWork.GetRepo<MaintenanceTracking>();
        _partService = partService;
    }

    #region Create
    public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
    {
        if (!await _vehicleModelRepo.ExistAsync(vehicle.ModelId))
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

    #region Create With histoy
    public async Task CreateVehicleWithHistoryAsync(
        Vehicle vehicle,
        List<MaintenanceTracking> maintenanceTracking
    )
    {
        if (!await _vehicleModelRepo.ExistAsync(vehicle.ModelId))
            throw new NotFoundException("Model Not Found");

        var partIds = maintenanceTracking.Select(mt => mt.PartId);
        await _partService.ValidatePartIdsExistAsync(partIds);

        await _vehicleRepo.CreateAsync(vehicle);
        //[NOTE]  i think calling omar emthod , to calculate is duo or allmost due and next maintence date and km
        await _trackingRepo.AddRangeAsync(maintenanceTracking);
        await _unitOfWork.SaveChanges();
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
        if (!await _vehicleRepo.ExistAsync(vehicle.Id))
            throw new NotFoundException("Vehicle Not Found");
        if (!await _vehicleModelRepo.ExistAsync(vehicle.ModelId))
            throw new NotFoundException("Model Not Found");

        _vehicleRepo.Update(vehicle);
        await _unitOfWork.SaveChanges();

        var spec = new VehicleIncludesSpecification(vehicle.Id);
        var returnVehicle = await _vehicleRepo.GetByIdWithSpecificationAsync(spec);
        return returnVehicle!;
    }
    #endregion
}
