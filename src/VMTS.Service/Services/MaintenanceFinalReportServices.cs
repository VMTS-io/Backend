using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications.Maintenance.Report.Final;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class MaintenanceFinalReportServices : IMaintenanceFinalReportServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<MaintenanceFinalReport> _finalReportRepo;
    private readonly IGenericRepository<MaintenanceInitialReport> _initialReportRepo;
    private readonly IGenericRepository<BusinessUser> _userRepo;
    private readonly IGenericRepository<Vehicle> _vehicleRepo;
    private readonly IGenericRepository<MaintenaceRequest> _requestRepo;
    private readonly IGenericRepository<Part> _partRepo;
    private readonly IGenericRepository<MaintenaceCategory> _categoryRepo;

    public MaintenanceFinalReportServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _initialReportRepo = _unitOfWork.GetRepo<MaintenanceInitialReport>();
        _userRepo = _unitOfWork.GetRepo<BusinessUser>();
        _vehicleRepo = _unitOfWork.GetRepo<Vehicle>();
        _requestRepo = _unitOfWork.GetRepo<MaintenaceRequest>();
        _partRepo = _unitOfWork.GetRepo<Part>();
        _categoryRepo = _unitOfWork.GetRepo<MaintenaceCategory>();
        _finalReportRepo = _unitOfWork.GetRepo<MaintenanceFinalReport>();
    }

    #region Create
    public async Task CreateFinalReportAsync(
        MaintenanceFinalReport report,
        List<string> categoryIds,
        List<string> partIds
    )
    {
        var validatedReport = await ValidateAndResolveAsync(report, categoryIds, partIds);

        validatedReport.MaintenaceRequest.Status = Status.Completed;
        _requestRepo.Update(validatedReport.MaintenaceRequest);

        await _finalReportRepo.CreateAsync(validatedReport);

        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Update
    public async Task UpdateFinalReportAsync(
        MaintenanceFinalReport updatedReport,
        List<string> categoryIds,
        List<string> partIds
    )
    {
        await GetReportOrThrowAsync(updatedReport.Id);
        var validatedReport = await ValidateAndResolveAsync(updatedReport, categoryIds, partIds);
        _finalReportRepo.Update(validatedReport);
        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Get All
    public async Task<IReadOnlyList<MaintenanceFinalReport>> GetAllFinalReportsAsync(
        MaintenanceFinalReportSpecParams specParams
    )
    {
        var spec = new MaintenanceFinalReportSpecification(specParams);
        return await _finalReportRepo.GetAllWithSpecificationAsync(spec);
    }
    #endregion

    #region Get By Id
    public async Task<MaintenanceFinalReport> GetFinalReportByIdAsync(string id)
    {
        return await GetReportWithSpecificationOrThrowAsync(id);
    }
    #endregion

    #region Delete
    public async Task DeleteFinalReportAsync(string id)
    {
        var report = await GetReportOrThrowAsync(id);
        _finalReportRepo.Delete(report);
        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Get or Throw
    private async Task<MaintenanceFinalReport> GetReportOrThrowAsync(string id)
    {
        return await _finalReportRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Final Report with ID {id} doesn't exist");
    }
    #endregion

    #region Get With Specifications or Throw
    private async Task<MaintenanceFinalReport> GetReportWithSpecificationOrThrowAsync(string id)
    {
        var spec = new MaintenanceFinalReportSpecification(id);
        return await _finalReportRepo.GetByIdWithSpecificationAsync(spec)
            ?? throw new NotFoundException($"Final Report with ID {id} doesn't exist");
    }
    #endregion

    #region Validate And Resolve
    private async Task<MaintenanceFinalReport> ValidateAndResolveAsync(
        MaintenanceFinalReport report,
        List<string> categoryIds,
        List<string> partIds
    )
    {
        // Validate foreign keys
        // report.Manager =
        //     await _userRepo.GetByIdAsync(report.ManagerId)
        //     ?? throw new NotFoundException($"Manager with ID {report.ManagerId} not found");

        report.InitialReport =
            await _initialReportRepo.GetByIdAsync(report.InitialReportId)
            ?? throw new NotFoundException(
                $"Initial Report with ID {report.InitialReportId} not found"
            );

        report.MaintenaceRequest =
            await _requestRepo.GetByIdAsync(report.InitialReport.MaintenanceRequestId)
            ?? throw new NotFoundException(
                $"Request with ID {report.MaintenaceRequestId} not found"
            );

        report.Mechanic =
            await _userRepo.GetByIdAsync(report.MaintenaceRequest.MechanicId)
            ?? throw new NotFoundException($"Mechanic with ID {report.MechanicId} not found");

        report.Vehicle =
            await _vehicleRepo.GetByIdAsync(report.MaintenaceRequest.VehicleId)
            ?? throw new NotFoundException($"Vehicle with ID {report.VehicleId} not found");

        // Validate and attach categories
        var foundCategories = await _categoryRepo.GetByIdsAsync(categoryIds);
        if (foundCategories.Count != categoryIds.Count)
        {
            var missing = categoryIds.Except(foundCategories.Select(c => c.Id));
            throw new NotFoundException($"Missing categories: {string.Join(", ", missing)}");
        }
        report.MaintenanceCategories = [.. foundCategories];

        // Validate and attach parts (optional)
        var foundParts = await _partRepo.GetByIdsAsync(partIds);
        if (foundParts.Count != partIds.Count)
        {
            var missing = partIds.Except(foundParts.Select(p => p.Id));
            throw new NotFoundException($"Missing parts: {string.Join(", ", missing)}");
        }
        report.ChangedParts = [.. foundParts];

        return report;
    }

    #endregion
}
