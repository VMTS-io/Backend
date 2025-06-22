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
    public async Task CreateInitialReportAsync(MaintenanceInitialReport report)
    {
        var validatedReport = await ValidateAndResolveAsync(report);

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
    public async Task UpdateInitialReportAsync(MaintenanceInitialReport updatedReport)
    {
        var report = await GetReportOrThrowAsync(updatedReport.Id);
        // updatedReport.ManagerId = report.ManagerId;
        updatedReport.MechanicId = report.MechanicId;
        var validatedReport = await ValidateAndResolveAsync(updatedReport);
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
        MaintenanceInitialReport report
    )
    {
        var maintenanceRequest =
            await _requestRepo.GetByIdAsync(report.MaintenanceRequestId)
            ?? throw new NotFoundException(
                $"Request with ID {report.MaintenanceRequestId} not found"
            );
        var initialReportSepc = new MaintenanceIntialReportSpecification(mir =>
            mir.MaintenanceRequestId == report.MaintenanceRequestId
        );
        var initialReport = _reportRepo.GetAllWithSpecificationAsync(initialReportSepc);
        if (initialReport is not null)
            throw new ConflictException(
                "There is already Finial report for this maintenance Request"
            );

        if (maintenanceRequest.InitialReport is not null)
            throw new ConflictException(
                "There is already Initial report for this maintenance Request"
            );

        if (maintenanceRequest.MechanicId != report.MechanicId)
            throw new BadRequestException(
                $"This Is Not {report.MechanicId}'s task, it is {report.MaintenanceRequest.MechanicId}"
            );

        if (!await _userRepo.ExistAsync(report.MechanicId))
            throw new NotFoundException($"Mechanic with ID {report.MechanicId} not found");

        if (!await _vehicleRepo.ExistAsync(maintenanceRequest.VehicleId))
            throw new NotFoundException($"Vehicle with ID {report.VehicleId} not found");
        report.VehicleId = maintenanceRequest.VehicleId;

        if (!await _categoryRepo.ExistAsync(maintenanceRequest.MaintenanceCategoryId))
            throw new NotFoundException($"Vehicle with ID {report.VehicleId} not found");
        report.MaintenanceCategoryId = maintenanceRequest.MaintenanceCategoryId;

        if (report.ExpectedChangedParts is null || !report.ExpectedChangedParts.Any())
            throw new BadRequestException("ExpectedChangedParts cannot be null or empty.");

        var partIds = report.ExpectedChangedParts.Select(cp => cp.PartId).ToHashSet();
        var foundParts = await _partRepo.GetByIdsAsync(partIds);
        var foundPartsDict = foundParts.ToDictionary(p => p.Id);

        // Validate missing parts
        var missingIds = partIds.Except(foundPartsDict.Keys).ToList();
        if (missingIds.Count != 0)
        {
            throw new NotFoundException(
                $"The following part IDs do not exist: {string.Join(", ", missingIds)}"
            );
        }

        // Check stock availability
        var outOfStockParts = new List<Part>();
        decimal totalExpectedCost = 0;

        foreach (var reportPart in report.ExpectedChangedParts)
        {
            var part = foundPartsDict[reportPart.PartId];

            if (part.Quantity < reportPart.Quantity)
                outOfStockParts.Add(part);

            reportPart.MaintnenanceInitialReportId = report.Id;
            totalExpectedCost += part.Cost * reportPart.Quantity;
        }

        report.MissingParts = outOfStockParts;
        report.ExpectedCost = totalExpectedCost;

        // var partIds = report.ExpectedChangedParts.Select(ecp => ecp.PartId).ToList();
        // var foundParts = await _partRepo.GetByIdsAsync(partIds);
        // if (foundParts.Count != partIds.Count)
        // {
        //     var missingFromDb = partIds.Except(foundParts.Select(p => p.Id));
        //     throw new NotFoundException(
        //         $"The following part IDs do not exist: {string.Join(", ", missingFromDb)}"
        //     );
        // }
        // var outOfStockParts = foundParts
        //     .Where(p =>
        //         p.Quantity < report.ExpectedChangedParts.First(ecp => ecp.PartId == p.Id).Quantity
        //     )
        //     .ToHashSet();
        // report.MissingParts = outOfStockParts;
        // foreach (var reportPart in report.ExpectedChangedParts)
        // {
        //     reportPart.MaintnenanceInitialReportId = report.Id;
        //     report.ExpectedCost +=
        //         reportPart.Quantity
        //         * foundParts.FirstOrDefault(p => p.Id == reportPart.PartId)!.Cost;
        // }
        return report;
    }
    #endregion
}
