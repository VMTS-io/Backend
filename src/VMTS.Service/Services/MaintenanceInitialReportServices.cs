using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications.Maintenance.Report.Initial;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class MaintenanceInitialReportServices : IMaintenanceInitialReportServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<MaintenanceInitialReport> _reportRepo;
    private readonly IGenericRepository<BusinessUser> _userRepo;
    private readonly IGenericRepository<Vehicle> _vehicleRepo;
    private readonly IGenericRepository<MaintenaceRequest> _requestRepo;
    private readonly IGenericRepository<Part> _partRepo;
    private readonly IGenericRepository<MaintenaceCategory> _categoryRepo;

    public MaintenanceInitialReportServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _reportRepo = _unitOfWork.GetRepo<MaintenanceInitialReport>();
        _userRepo = _unitOfWork.GetRepo<BusinessUser>();
        _vehicleRepo = _unitOfWork.GetRepo<Vehicle>();
        _requestRepo = _unitOfWork.GetRepo<MaintenaceRequest>();
        _partRepo = _unitOfWork.GetRepo<Part>();
        _categoryRepo = _unitOfWork.GetRepo<MaintenaceCategory>();
    }

    #region Create
    public async Task CreateInitialReportAsync(
        MaintenanceInitialReport report,
        List<string> categoryIds,
        List<string>? partIds
    )
    {
        var validatedReport = await ValidateAndResolveAsync(report, categoryIds, partIds);

        if (validatedReport.MissingParts is null || validatedReport.MissingParts.Count == 0)
        {
            validatedReport.MaintenanceRequest.Status = Status.InProgress;
            _requestRepo.Update(validatedReport.MaintenanceRequest);
        }
        await _reportRepo.CreateAsync(validatedReport);
        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Update
    public async Task UpdateInitialReportAsync(
        MaintenanceInitialReport updatedReport,
        List<string> categoryIds,
        List<string>? partIds
    )
    {
        var report = await GetReportOrThrowAsync(updatedReport.Id);
        // updatedReport.ManagerId = report.ManagerId;
        updatedReport.MechanicId = report.MechanicId;
        var validatedReport = await ValidateAndResolveAsync(updatedReport, categoryIds, partIds);
        _reportRepo.Update(validatedReport);
        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Get All
    public async Task<IReadOnlyList<MaintenanceInitialReport>> GetAllInitialReportsAsync(
        MaintenanceIntialReportSpecParams specParams
    )
    {
        var specs = new MaintenanceIntialReportSpecification(specParams);
        return await _reportRepo.GetAllWithSpecificationAsync(specs);
    }
    #endregion

    #region Get By Id
    public async Task<MaintenanceInitialReport> GetInitialReportByIdAsync(string id)
    {
        return await GetReportWithSpecsOrThrowAsync(id);
    }
    #endregion

    #region Delete
    public async Task DeleteInitialReportAsync(string id)
    {
        var report = await GetReportOrThrowAsync(id);
        _reportRepo.Delete(report);
        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Get or throw
    private async Task<MaintenanceInitialReport> GetReportOrThrowAsync(string id)
    {
        return await _reportRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Initial Report with ID {id} doesn't exist");
    }
    #endregion

    #region Get With Specs or throw
    private async Task<MaintenanceInitialReport> GetReportWithSpecsOrThrowAsync(string id)
    {
        var specs = new MaintenanceIntialReportSpecification(id);
        return await _reportRepo.GetByIdWithSpecificationAsync(specs)
            ?? throw new NotFoundException($"Initial Report with ID {id} doesn't exist");
    }
    #endregion

    #region Velidate And Resolve
    private async Task<MaintenanceInitialReport> ValidateAndResolveAsync(
        MaintenanceInitialReport report,
        List<string> categoryIds,
        List<string>? partIds
    )
    {
        report.MaintenanceRequest =
            await _requestRepo.GetByIdAsync(report.MaintenanceRequestId)
            ?? throw new NotFoundException(
                $"Request with ID {report.MaintenanceRequestId} not found"
            );

        if (report.MaintenanceRequest.MechanicId != report.MechanicId)
            throw new BadRequestException(
                $"This Is Not {report.MechanicId}'s task, it is {report.MaintenanceRequest.ManagerId}"
            );

        report.Mechanic =
            await _userRepo.GetByIdAsync(report.MechanicId)
            ?? throw new NotFoundException($"Mechanic with ID {report.MechanicId} not found");

        // report.Manager =
        //     await _userRepo.GetByIdAsync(report.MaintenaceRequest.ManagerId)
        //     ?? throw new NotFoundException($"Manager with ID {report.ManagerId} not found");

        report.Vehicle =
            await _vehicleRepo.GetByIdAsync(report.MaintenanceRequest.VehicleId)
            ?? throw new NotFoundException($"Vehicle with ID {report.VehicleId} not found");

        // report.VehicleId = report.MaintenaceRequest.VehicleId;
        // report.ManagerId = report.MaintenaceRequest.ManagerId;
        // Validate and attach categories
        var foundCategories = await _categoryRepo.GetByIdsAsync(categoryIds);
        if (foundCategories.Count != categoryIds.Count)
        {
            var missing = categoryIds.Except(foundCategories.Select(c => c.Id));
            throw new NotFoundException($"Missing categories: {string.Join(", ", missing)}");
        }
        report.MaintenanceCategories = [.. foundCategories];

        // Validate and attach parts (optional)
        if (partIds is not null)
        {
            var foundParts = await _partRepo.GetByIdsAsync(partIds);
            if (foundParts.Count != partIds.Count)
            {
                var missing = partIds.Except(foundParts.Select(p => p.Id));
                throw new NotFoundException($"Missing parts: {string.Join(", ", missing)}");
            }
            report.MissingParts = [.. foundParts];
        }

        return report;
    }
    #endregion
}
