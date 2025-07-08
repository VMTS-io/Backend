using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications;
using VMTS.Core.Specifications.Maintenance.Report.Final;
using VMTS.Core.Specifications.TripRequestSpecification;
using VMTS.Core.Specifications.VehicleSpecification;
using VMTS.Core.Specifications.VehicleSpecification.VehicleModelSpecification;

namespace VMTS.Service.Services;

public class DashboardServices : IDashboardServices
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> GetCountTripsWithFaultsAsync()
    {
        var specs = new TripRequestIncludesSpecification(tr => tr.FaultReports != null);
        ;
        var tripsWithoutFault = await _unitOfWork.GetRepo<TripRequest>().GetCountAsync(specs);
        return tripsWithoutFault;
    }

    public async Task<int> GetCountTripsWithoutFaultsAsync()
    {
        var specs = new TripRequestIncludesSpecification(tr =>
            tr.FaultReports == null && tr.TripReports != null
        );
        ;
        var tripsWithoutFault = await _unitOfWork.GetRepo<TripRequest>().GetCountAsync(specs);
        return tripsWithoutFault;
    }

    public async Task<decimal> GetTotalMaintenanceCostAsync()
    {
        var specs = new MaintenanceFinalReportSpecification(tr =>
            tr.FinishedDate > DateTime.UtcNow.AddMonths(-1)
        );
        var cost = await _unitOfWork
            .GetRepo<MaintenanceFinalReport>()
            .GetAllWithSpecificationAsync(specs);
        var totalCost = cost.Sum(c => c.TotalCost);
        return totalCost;
    }

    public async Task<decimal> GetTotalFuelCostAsync()
    {
        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

        var tripSpecs = new TripReportIncludesSpecification(tr => tr.ReportedAt > oneMonthAgo);
        var tripReports = await _unitOfWork
            .GetRepo<TripReport>()
            .GetAllWithSpecificationAsync(tripSpecs);

        var faultSpecs = new FaultReportIncludesSpecification(fr => fr.ReportedAt > oneMonthAgo);
        var faultReports = await _unitOfWork
            .GetRepo<FaultReport>()
            .GetAllWithSpecificationAsync(faultSpecs);

        var tripFuelCost = tripReports.Sum(tr => tr.FuelCost);
        var faultFuelCost = faultReports.Sum(fr => fr.Cost);

        return tripFuelCost + faultFuelCost;
    }

    public Task<int> GetTotalTripsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetTotalVehicleCount()
    {
        var specs = new VehicleIncludesSpecification(v => v.Status != VehicleStatus.Retired);
        var vehicles = await _unitOfWork.GetRepo<Vehicle>().GetCountAsync(specs);
        return vehicles;
    }

    public async Task<int> GetVehicleUnderMaintenanceCount()
    {
        var specs = new VehicleIncludesSpecification(v =>
            v.Status == VehicleStatus.UnderMaintenance
        );
        var vehicles = await _unitOfWork.GetRepo<Vehicle>().GetCountAsync(specs);
        return vehicles;
    }

    public async Task<int> GetVehicleAvailableCount()
    {
        var specs = new VehicleIncludesSpecification(v => v.Status == VehicleStatus.Available);
        var vehicles = await _unitOfWork.GetRepo<Vehicle>().GetCountAsync(specs);
        return vehicles;
    }
}
