using System.Security.Claims;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications.Maintenance;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class MaintenanceRequestServices : IMaintenanceRequestServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<MaintenaceRequest> _repo;
    private readonly IGenericRepository<BusinessUser> _userRepo;
    private readonly IGenericRepository<Vehicle> _vehicleRepo;

    public MaintenanceRequestServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repo = _unitOfWork.GetRepo<MaintenaceRequest>();
        _userRepo = _unitOfWork.GetRepo<BusinessUser>();
        _vehicleRepo = _unitOfWork.GetRepo<Vehicle>();
    }

    #region Create
    public async Task CreateAsync(MaintenaceRequest model)
    {
        if (!await _userRepo.ExistAsync(model.ManagerId))
            throw new Exception("Manager Not Found");

        var mechanic =
            await _userRepo.GetByIdAsync(model.MechanicId)
            ?? throw new NotFoundException($"Mechanic Not Found With ID {model.Id}");
        if (mechanic.Role != Roles.Mechanic)
            throw new ConflictException("User With ID {mechanic.Id} is not a mechanic");

        if (!await _vehicleRepo.ExistAsync(model.VehicleId))
            throw new Exception("Vechile Not Found");

        await _repo.CreateAsync(model);
        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Update
    public async Task UpdateAsync(MaintenaceRequest model)
    {
        var existingRequest =
            await _repo.GetByIdAsync(model.Id)
            ?? throw new NotFoundException($"No MaintenaceRequest With ID {model.Id}");

        if (!await _vehicleRepo.ExistAsync(model.VehicleId))
            throw new Exception("Vechile Not Found");

        model.ManagerId = existingRequest.ManagerId;
        model.Status = existingRequest.Status;
        model.Date = existingRequest.Date;

        _repo.Update(model);
        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Get All
    public async Task<IReadOnlyList<MaintenaceRequest>> GetAllAsync(
        MaintenanceRequestSpecParams specParams
    )
    {
        var spec = new MaintenanceRequestSpecification(specParams);
        var result = await _repo.GetAllWithSpecificationAsync(spec);
        return result;
    }
    #endregion

    #region Get All For User
    public async Task<IReadOnlyList<MaintenaceRequest>> GetAllForUserAsync(
        MaintenanceRequestSpecParamsForMechanic mechanicSpecParams,
        string mechanicId
    )
    {
        //if the user is deleted but still have a token
        if (!await _userRepo.ExistAsync(mechanicId!))
            throw new Exception("Mechanic Not Found");

        var specParam = new MaintenanceRequestSpecParams(mechanicSpecParams)
        {
            MechanicId = mechanicId,
        };
        var spec = new MaintenanceRequestSpecification(specParam);
        return await _repo.GetAllWithSpecificationAsync(spec);
    }
    #endregion

    #region Get By Id
    public async Task<MaintenaceRequest> GetByIdAsync(string id)
    {
        var spec = new MaintenanceRequestSpecification(id);
        return await _repo.GetByIdWithSpecificationAsync(spec)
            ?? throw new NotFoundException($"No Maintenace Request With Id {id}");
    }
    #endregion

    #region Delete
    public async Task DeleteAsync(string id)
    {
        var request =
            await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"No Maintenace Request With Id {id}");
        _repo.Delete(request);
        await _unitOfWork.SaveChanges();
    }
    #endregion
}
