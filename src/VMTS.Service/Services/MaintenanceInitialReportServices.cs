using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications;
using VMTS.Core.Specifications.Maintenance.Report;
using VMTS.Core.Specifications.Maintenance.Report.Initial;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class MaintenanceInitialReportServices : IMaintenanceInitialReportServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<MaintenanceInitialReport> _reportRepo;
    private readonly IGenericRepository<MaintenaceRequest> _requestRepo;
    private readonly IGenericRepository<Part> _partRepo;
    private readonly IPartService _partServices;

    public MaintenanceInitialReportServices(IUnitOfWork unitOfWork, IPartService partServices)
    {
        _unitOfWork = unitOfWork;
        _reportRepo = _unitOfWork.GetRepo<MaintenanceInitialReport>();
        _requestRepo = _unitOfWork.GetRepo<MaintenaceRequest>();
        _partRepo = _unitOfWork.GetRepo<Part>();
        _partServices = partServices;
    }

    #region Create
    public async Task CreateInitialReportAsync(MaintenanceInitialReport report)
    {
        var validatedReport = await ValidateAndResolveAsync(report);

        if (validatedReport.MissingParts is null || validatedReport.MissingParts.Count == 0)
        {
            validatedReport.MaintenanceRequest.Status = MaintenanceStatus.InProgress;
            _requestRepo.Update(validatedReport.MaintenanceRequest);
        }
        await _reportRepo.CreateAsync(validatedReport);
        await _unitOfWork.SaveChangesAsync();
    }
    #endregion

    #region Update
    public async Task UpdateInitialReportAsync(MaintenanceInitialReport updatedReport)
    {
        var spec = new BaseSpecification<MaintenanceInitialReport>()
        {
            Criteria = mir => mir.Id == updatedReport.Id,
            Includes =
            [
                mir => mir.MaintenanceRequest,
                mir => mir.MissingParts,
                mir => mir.ExpectedChangedParts,
            ],
        };
        var existingReport =
            await _reportRepo.GetByIdWithSpecificationAsync(spec)
            ?? throw new NotFoundException($"No Report With ID {updatedReport.Id}");

        updatedReport.MechanicId = existingReport.MechanicId;
        updatedReport.MaintenanceRequestId = existingReport.MaintenanceRequestId;
        updatedReport.MaintenanceCategory = existingReport.MaintenanceCategory;
        await ValidateAndApplyUpdateAsync(existingReport, updatedReport);
        _reportRepo.Update(existingReport);
        await _unitOfWork.SaveChangesAsync();
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

    #region Get All For Joint EndPoint
    public async Task<IReadOnlyList<MaintenanceInitialReport>> GetAllInitialReportsAsync(
        MaintenanceReportSpecParams specParams
    )
    {
        var spec = new MaintenanceIntialReportSpecification(specParams);
        return await _reportRepo.GetAllWithSpecificationAsync(spec);
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
        await _unitOfWork.SaveChangesAsync();
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
        var maintencRequstSpec = new BaseSpecification<MaintenaceRequest>()
        {
            Criteria = mr => mr.Id == report.MaintenanceRequestId,
            Includes = [mr => mr.InitialReport],
        };
        var maintenanceRequest =
            await _requestRepo.GetByIdWithSpecificationAsync(maintencRequstSpec)
            ?? throw new NotFoundException(
                $"Request with ID {report.MaintenanceRequestId} not found"
            );
        if (maintenanceRequest.InitialReport is not null)
            throw new ConflictException(
                "There is already Initial report for this maintenance Request"
            );

        if (maintenanceRequest.MechanicId != report.MechanicId)
            throw new BadRequestException(
                $"This Is Not {report.MechanicId}'s task, it is {report.MaintenanceRequest.MechanicId}"
            );

        report.MaintenanceRequest = maintenanceRequest;
        report.VehicleId = maintenanceRequest.VehicleId;
        report.MaintenanceCategory = maintenanceRequest.MaintenanceCategory;

        // Check stock availability
        var partIds = report.ExpectedChangedParts.Select(cp => cp.PartId).ToHashSet();
        var foundPartsDict = await _partServices.ValidatePartIdsExistAsync(partIds);
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

        return report;
    }
    #endregion

    #region Validate And Update
    private async Task ValidateAndApplyUpdateAsync(
        MaintenanceInitialReport existing,
        MaintenanceInitialReport updated
    )
    {
        var incomingParts = updated.ExpectedChangedParts;

        var partIds = incomingParts.Select(p => p.PartId).ToHashSet();
        var foundDict = await _partServices.ValidatePartIdsExistAsync(partIds);

        var outOfStockParts = new List<Part>();
        var newPartMap = incomingParts.ToDictionary(p => p.PartId, p => p.Quantity);
        decimal expectedCost = 0;

        // Remove deleted parts
        var toRemove = existing
            .ExpectedChangedParts.Where(p => !newPartMap.ContainsKey(p.PartId))
            .ToList();
        foreach (var r in toRemove)
        {
            existing.ExpectedChangedParts.Remove(r);
        }

        // Update or add parts
        foreach (var (partId, qty) in newPartMap)
        {
            var existingPart = existing.ExpectedChangedParts.FirstOrDefault(p =>
                p.PartId == partId
            );
            if (existingPart != null)
            {
                existingPart.Quantity = qty;
            }
            else
            {
                existing.ExpectedChangedParts.Add(
                    new MaintenanceInitialReportParts
                    {
                        MaintnenanceInitialReportId = existing.Id,
                        PartId = partId,
                        Quantity = qty,
                    }
                );
            }

            var part = foundDict[partId];
            if (part.Quantity < qty)
                outOfStockParts.Add(part);

            expectedCost += part.Cost * qty;
        }

        existing.Notes = updated.Notes;
        existing.ExpectedFinishDate = updated.ExpectedFinishDate;
        existing.ExpectedCost = expectedCost;
        existing.MissingParts = outOfStockParts;
    }
    #endregion

    #region mark as seen

    public async Task UpdateMarkAsSeen(string initialReportId)
    {
        var initialReport = await _unitOfWork
            .GetRepo<MaintenanceInitialReport>()
            .GetByIdAsync(initialReportId);
        if (initialReport is null)
            throw new NotFoundException("Initial Report Not Found");
        if (initialReport.Seen == false)
        {
            initialReport.Seen = true;
            _unitOfWork.GetRepo<MaintenanceInitialReport>().Update(initialReport);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    #endregion
}
