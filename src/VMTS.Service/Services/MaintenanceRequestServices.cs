using System.Security.Claims;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications.Maintenance;

namespace VMTS.Service.Services;

public class MaintenanceRequestServices : IMaintenanceRequestServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<MaintenaceRequest> _repo;

    public MaintenanceRequestServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repo = _unitOfWork.GetRepo<MaintenaceRequest>();
    }

    public async Task CreateAsync(MaintenaceRequest model, ClaimsPrincipal user)
    {
        var managerId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        model.ManagerId = managerId!;
        await _repo.CreateAsync(model);
        await _unitOfWork.SaveChanges();
    }

    public async Task UpdateAsync(MaintenaceRequest model)
    {
        _repo.Update(model);
        await _unitOfWork.SaveChanges();
    }

    public async Task<IReadOnlyList<MaintenaceRequest>> GetAllAsync(
        MaintenanceRequestSpecParams specParams
    )
    {
        var spec = new MaintenanceRequestSpecification(specParams);
        var result = await _repo.GetAllWithSpecification(spec);
        return result;
    }

    public async Task<IReadOnlyList<MaintenaceRequest>> GetAllForUserAsync(
        string mechanicId,
        MaintenanceRequestSpecParamsForMechanic mechanicSpecParams
    )
    {
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
