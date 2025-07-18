using System.Globalization;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Non_Entities_Class;
using VMTS.Core.Specifications;
using VMTS.Core.Specifications.Maintenance.Report.Final;
using VMTS.Core.Specifications.TripRequestSpecification;
using VMTS.Core.Specifications.VehicleSpecification;

namespace VMTS.Service.Services;

public class DashboardServices : IDashboardServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAiClient _aiClient;

    public DashboardServices(IUnitOfWork unitOfWork, IAiClient aiClient)
    {
        _unitOfWork = unitOfWork;
        _aiClient = aiClient;
    }

    public async Task<int> GetCountTripsWithFaultsAsync(int? month)
    {
        if (month is null)
            month = DateTime.UtcNow.Month;

        var year = DateTime.UtcNow.Year;

        var specs = new TripRequestIncludesSpecification(tr =>
            tr.FaultReports != null && tr.Date.Month == month && tr.Date.Year == year
        );
        ;
        var tripsWithoutFault = await _unitOfWork.GetRepo<TripRequest>().GetCountAsync(specs);
        return tripsWithoutFault;
    }

    public async Task<int> GetCountTripsWithoutFaultsAsync(int? month)
    {
        if (month is null)
            month = DateTime.UtcNow.Month;

        var year = DateTime.UtcNow.Year;
        var specs = new TripRequestIncludesSpecification(tr =>
            tr.FaultReports == null
            && tr.TripReports != null
            && tr.Date.Month == month
            && tr.Date.Year == year
        );
        ;
        var tripsWithoutFault = await _unitOfWork.GetRepo<TripRequest>().GetCountAsync(specs);
        return tripsWithoutFault;
    }

    public async Task<decimal> GetTotalMaintenanceCostAsync(int? month)
    {
        if (month is null)
            month = DateTime.UtcNow.Month;

        var year = DateTime.UtcNow.Year;

        var specs = new MaintenanceFinalReportSpecification(tr =>
            tr.FinishedDate.Year == year && tr.FinishedDate.Month == month
        );

        var totalCost = await _unitOfWork
            .GetRepo<MaintenanceFinalReport>()
            .SumWithSpecificationAsync(specs, c => c.TotalCost);

        return totalCost;
    }

    public async Task<decimal> GetTotalFuelCostAsync(int? month)
    {
        if (month is null)
            month = DateTime.UtcNow.Month;

        var year = DateTime.UtcNow.Year;
        // var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

        var tripSpecs = new TripReportIncludesSpecification(tr =>
            tr.ReportedAt.Year == year && tr.ReportedAt.Month == month
        );
        var tripFuelCost = await _unitOfWork
            .GetRepo<TripReport>()
            .SumWithSpecificationAsync(tripSpecs, tr => tr.FuelCost);

        var faultSpecs = new FaultReportIncludesSpecification(fr =>
            fr.ReportedAt.Year == year && fr.ReportedAt.Month == month
        );
        var faultFuelCost = await _unitOfWork
            .GetRepo<FaultReport>()
            .SumWithSpecificationAsync(faultSpecs, fr => fr.Cost);

        // var tripFuelCost = tripReports.Sum(tr => tr.FuelCost);
        // var faultFuelCost = faultReports.Sum(fr => fr.Cost);

        return tripFuelCost + faultFuelCost;
    }

    public async Task<int> GetTotalTripsAsync(int? month)
    {
        var taotalTrips = await _unitOfWork.GetRepo<TripRequest>().GetAllAsync();
        return taotalTrips.Count;
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

    public async Task<CostChartDto> GetTimeSeriesCostChartAsync()
    {
        var months = Enumerable
            .Range(0, 6)
            .Select(i => DateTime.Now.AddMonths(-i))
            .OrderBy(d => d)
            .ToArray();

        var monthLabels = months
            .Select(d => d.ToString("MMM", CultureInfo.InvariantCulture))
            .ToArray();
        var fuelCosts = new List<decimal>();
        var maintenanceCosts = new List<decimal>();

        foreach (var date in months)
        {
            int month = date.Month;
            fuelCosts.Add(await GetTotalFuelCostAsync(month));
            maintenanceCosts.Add(await GetTotalMaintenanceCostAsync(month));
        }

        var data = new MonthlyCostsChartDto
        {
            Months = monthLabels,
            FuelCosts = fuelCosts.ToArray(),
            MaintenanceCosts = maintenanceCosts.ToArray(),
        };

        return await _aiClient.SendMonthlyCostsAndGetChartAsync(data);
    }
}
