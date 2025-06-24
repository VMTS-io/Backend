using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications;
using VMTS.Core.Specifications.Maintenance.Report;
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

    public MaintenanceFinalReportServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _initialReportRepo = _unitOfWork.GetRepo<MaintenanceInitialReport>();
        _userRepo = _unitOfWork.GetRepo<BusinessUser>();
        _vehicleRepo = _unitOfWork.GetRepo<Vehicle>();
        _requestRepo = _unitOfWork.GetRepo<MaintenaceRequest>();
        _partRepo = _unitOfWork.GetRepo<Part>();
        _finalReportRepo = _unitOfWork.GetRepo<MaintenanceFinalReport>();
    }

    #region Create
    public async Task CreateFinalReportAsync(MaintenanceFinalReport report)
    {
        var validatedReport = await ValidateAndResolveAsync(report);

        validatedReport.MaintenaceRequest.Status = Status.Completed;
        _requestRepo.Update(validatedReport.MaintenaceRequest);

        await _finalReportRepo.CreateAsync(validatedReport);

        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Update
    public async Task UpdateFinalReportAsync(MaintenanceFinalReport updatedReport)
    {
        var spec = new BaseSpecification<MaintenanceFinalReport>
        {
            Criteria = r => r.Id == updatedReport.Id,
            Includes = [r => r.ChangedParts],
        };

        var existingReport =
            await _finalReportRepo.GetByIdWithSpecificationAsync(spec)
            ?? throw new NotFoundException("Final report not found");
        updatedReport.InitialReportId = existingReport.InitialReportId;
        updatedReport.MaintenaceRequestId = existingReport.MaintenaceRequestId;
        updatedReport.MechanicId = existingReport.MechanicId;
        updatedReport.MaintenanceCategoryId = existingReport.MaintenanceCategoryId;

        await ValidateAndApplyUpdateAsync(existingReport, updatedReport);

        _finalReportRepo.Update(existingReport);
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

    #region Get All For Joint Endpoint

    public async Task<IReadOnlyList<MaintenanceFinalReport>> GetAllFinalReportsAsync(
        MaintenanceReportSpecParams specParams
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
        MaintenanceFinalReport report
    )
    {
        var initialReport =
            await _initialReportRepo.GetByIdAsync(report.InitialReportId)
            ?? throw new NotFoundException(
                $"Initial Report with ID {report.InitialReportId} not found"
            );
        var maintenanceRequestSpec = new BaseSpecification<MaintenaceRequest>()
        {
            Criteria = mr => mr.Id == initialReport.MaintenanceRequestId,
            Includes = [mr => mr.FinalReport],
        };
        var maintenanceRequest =
            await _requestRepo.GetByIdWithSpecificationAsync(maintenanceRequestSpec)
            ?? throw new NotFoundException(
                $"Request with ID {report.MaintenaceRequestId} not found"
            );
        if (maintenanceRequest.FinalReport is not null)
            throw new ConflictException("There is already a final report for this request.");

        if (maintenanceRequest.MechanicId != report.MechanicId)
            throw new ConflictException("Mechanic mismatch");

        report.MaintenaceRequestId = maintenanceRequest.Id;
        report.MaintenaceRequest = maintenanceRequest;
        report.MaintenanceCategoryId = initialReport.MaintenanceCategoryId;
        report.VehicleId = maintenanceRequest.VehicleId;

        var partIds = report.ChangedParts.Select(p => p.PartId).ToHashSet();
        var foundParts = await _partRepo.GetByIdsAsync(partIds);
        var partDict = foundParts.ToDictionary(p => p.Id);

        var missingIds = partIds.Except(partDict.Keys).ToList();
        if (missingIds.Count > 0)
            throw new NotFoundException($"Missing part IDs: {string.Join(", ", missingIds)}");

        decimal totalCost = 0;

        foreach (var reportPart in report.ChangedParts)
        {
            reportPart.MaintnenanceFinalReportId = report.Id;

            var part = partDict[reportPart.PartId];
            if (part.Quantity < reportPart.Quantity)
                part.Quantity = 0;
            else
                part.Quantity -= reportPart.Quantity;
            _partRepo.Update(part);

            totalCost += reportPart.Quantity * part.Cost;
        }

        report.TotalCost = totalCost;

        return report;
    }
    #endregion

    #region Validate and Update
    private async Task ValidateAndApplyUpdateAsync(
        MaintenanceFinalReport existing,
        MaintenanceFinalReport updated
    )
    {
        var newParts = updated.ChangedParts;
        var partIds = newParts.Select(p => p.PartId).ToHashSet();

        var foundParts = await _partRepo.GetByIdsAsync(
            partIds.Union(existing.ChangedParts.Select(p => p.PartId))
        );
        var partDict = foundParts.ToDictionary(p => p.Id);

        var missing = partIds.Except(partDict.Keys).ToList();
        if (missing.Count > 0)
            throw new NotFoundException($"Missing parts: {string.Join(", ", missing)}");

        // Step 1: Rollback old quantities
        foreach (var old in existing.ChangedParts)
        {
            var part = partDict[old.PartId];
            part.Quantity += old.Quantity;
            _partRepo.Update(part);
        }

        // Step 2: Clear and reapply
        var newMap = newParts.ToDictionary(p => p.PartId, p => p.Quantity);
        var toRemove = existing.ChangedParts.Where(p => !newMap.ContainsKey(p.PartId)).ToList();
        foreach (var r in toRemove)
            existing.ChangedParts.Remove(r);

        decimal totalCost = 0;

        foreach (var (partId, qty) in newMap)
        {
            var part = partDict[partId];

            if (part.Quantity < qty)
                throw new BadRequestException($"Not enough stock for part {part.Name}");

            var existingPart = existing.ChangedParts.FirstOrDefault(p => p.PartId == partId);
            if (existingPart != null)
            {
                existingPart.Quantity = qty;
            }
            else
            {
                existing.ChangedParts.Add(
                    new MaintenanceFinalReportParts
                    {
                        MaintnenanceFinalReportId = existing.Id,
                        PartId = partId,
                        Quantity = qty,
                    }
                );
            }

            part.Quantity -= qty;
            _partRepo.Update(part);

            totalCost += qty * part.Cost;
        }

        existing.TotalCost = totalCost;
        existing.Notes = updated.Notes;
        existing.FinishedDate = updated.FinishedDate;
    }

    #endregion
}
