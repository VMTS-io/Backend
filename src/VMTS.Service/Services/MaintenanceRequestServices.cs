using System.Linq.Expressions;
using System.Security.Claims;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications.Maintenance;
using VMTS.Core.Specifications.Maintenance.Tracking;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class MaintenanceRequestServices : IMaintenanceRequestServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPartService _partServices;
    private readonly IGenericRepository<MaintenaceRequest> _repo;
    private readonly IGenericRepository<MaintenanceTracking> _trackingRepo;
    private readonly IGenericRepository<BusinessUser> _userRepo;
    private readonly IGenericRepository<Vehicle> _vehicleRepo;

    public MaintenanceRequestServices(IUnitOfWork unitOfWork, IPartService partServices)
    {
        _unitOfWork = unitOfWork;
        _partServices = partServices;
        _repo = _unitOfWork.GetRepo<MaintenaceRequest>();
        _userRepo = _unitOfWork.GetRepo<BusinessUser>();
        _vehicleRepo = _unitOfWork.GetRepo<Vehicle>();
        _trackingRepo = _unitOfWork.GetRepo<MaintenanceTracking>();
    }

    #region Create
    public async Task CreateAsync(MaintenaceRequest model, List<string> parts)
    {
        if (model.MaintenanceCategory == MaintenanceCategory.Regular)
        {
            if (parts.Count == 0)
                throw new BadRequestException(
                    "You can't send regular maintenance request without parts selected"
                );

            var partsDic = await _partServices.ValidatePartIdsExistAsync(parts);
            model.Parts = partsDic.Values.ToList();

            var trackingSpec = new MaintenanceTrackingSpecification()
            {
                Criteria = mt => mt.VehicleId == model.VehicleId && parts.Contains(mt.PartId),
            };
            var trackings = await _trackingRepo.GetAllWithSpecificationAsync(trackingSpec);

            foreach (var trackning in trackings)
            {
                trackning.IsAlmostDue = false;
                trackning.IsDue = false;
            }

            _trackingRepo.UpdateRange(trackings);
        }

        if (!await _userRepo.ExistAsync(model.ManagerId))
            throw new Exception("Manager Not Found");

        var mechanic =
            await _userRepo.GetByIdAsync(model.MechanicId)
            ?? throw new NotFoundException($"Mechanic Not Found With ID {model.Id}");
        if (mechanic.Role != Roles.Mechanic)
            throw new ConflictException("User With ID {mechanic.Id} is not a mechanic");

        var vehicle =
            await _vehicleRepo.GetByIdAsync(model.VehicleId)
            ?? throw new Exception("Vechile Not Found");

        vehicle.Status = VehicleStatus.OutOfService;

        _vehicleRepo.Update(vehicle);
        await _repo.CreateAsync(model);
        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Update
    public async Task UpdateAsync(MaintenaceRequest model, List<string> parts)
    {
        var existingRequest =
            await _repo.GetByIdAsync(model.Id)
            ?? throw new NotFoundException($"No MaintenaceRequest With ID {model.Id}");

        if (!await _vehicleRepo.ExistAsync(model.VehicleId))
            throw new Exception("Vechile Not Found");
        await _partServices.ValidatePartIdsExistAsync(model.Parts.Select(p => p.Id));

        model.ManagerId = existingRequest.ManagerId;
        model.Status = existingRequest.Status;
        model.Date = existingRequest.Date;

        var partsDic = await _partServices.ValidatePartIdsExistAsync(parts);
        model.Parts = partsDic.Values.ToList();

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
