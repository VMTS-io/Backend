using System.Security.Claims;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications.Maintenance;

namespace VMTS.Service.Services;

public class MaintenanceRequestServices : IMaintenanceRequestServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<MaintenaceRequest> _repo;
    private readonly IGenericRepository<BusinessUser> _userRepo;

    public MaintenanceRequestServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repo = _unitOfWork.GetRepo<MaintenaceRequest>();
        _userRepo = _unitOfWork.GetRepo<BusinessUser>();
    }

    public async Task<MaintenaceRequest> CreateAsync(MaintenaceRequest model, ClaimsPrincipal user)
    {
        var managerId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        _ = await _userRepo.GetByIdAsync(managerId!) ?? throw new Exception("Manager Not Found");
        model.ManagerId = managerId!;

        _ =
            await _userRepo.GetByIdAsync(model.MechanicId)
            ?? throw new Exception("Mechanic Not Found");

        _ =
            _unitOfWork.GetRepo<Vehicle>().GetByIdAsync(model.VehicleId)
            ?? throw new Exception("Vechile Not Found");

        await _repo.CreateAsync(model);
        await _unitOfWork.SaveChanges();

        var spec = new MaintenanceRequestSpecification(model.Id);
        var maintenaceRequest = await _repo.GetByIdWithSpecificationAsync(spec);
        return maintenaceRequest!;
    }

    public async Task<MaintenaceRequest> UpdateAsync(MaintenaceRequest model, ClaimsPrincipal user)
    {
        var managerId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        _ = await _userRepo.GetByIdAsync(managerId!) ?? throw new Exception("Manager Not Found");
        model.ManagerId = managerId!;

        _ =
            await _userRepo.GetByIdAsync(model.MechanicId)
            ?? throw new Exception("Mechanic Not Found");

        _ =
            _unitOfWork.GetRepo<Vehicle>().GetByIdAsync(model.VehicleId)
            ?? throw new Exception("Vechile Not Found");

        _repo.Update(model);
        await _unitOfWork.SaveChanges();

        var spec = new MaintenanceRequestSpecification(model.Id);
        var maintenaceRequest = await _repo.GetByIdWithSpecificationAsync(spec);
        return maintenaceRequest!;
    }

    public async Task<IReadOnlyList<MaintenaceRequest>> GetAllAsync(
        MaintenanceRequestSpecParams specParams
    )
    {
        var spec = new MaintenanceRequestSpecification(specParams);
        var result = await _repo.GetAllWithSpecificationAsync(spec);
        return result;
    }

    public async Task<IReadOnlyList<MaintenaceRequest>> GetAllForUserAsync(
        MaintenanceRequestSpecParamsForMechanic mechanicSpecParams,
        ClaimsPrincipal user
    )
    {
        var mechanicId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        _ = await _userRepo.GetByIdAsync(mechanicId!) ?? throw new Exception("Manager Not Found");
        var specParam = new MaintenanceRequestSpecParams()
        {
            PageSize = mechanicSpecParams.PageSize,
            Date = mechanicSpecParams.Date,
            Status = mechanicSpecParams.Status,
            OrderBy = mechanicSpecParams.OrderBy,
            PageIndex = mechanicSpecParams.PageIndex,
            VehicleId = mechanicSpecParams.VehicleId,
            MechanicId = mechanicId,
            Id = mechanicSpecParams.Id,
        };
        var spec = new MaintenanceRequestSpecification(specParam);
        var result = await _repo.GetAllWithSpecificationAsync(spec);
        return result;
    }

    public async Task<MaintenaceRequest?> GetByIdAsync(string id)
    {
        var spec = new MaintenanceRequestSpecification(id);
        var result = await _repo.GetByIdWithSpecificationAsync(spec);
        return result;
    }
}
