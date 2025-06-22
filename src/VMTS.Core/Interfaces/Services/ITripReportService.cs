using VMTS.Core.Entities.Trip;
using VMTS.Core.Specifications;

namespace VMTS.Core.Interfaces.Services;

public interface ITripReportService
{
    Task<TripReport> CreateTripReportAsync(
        string driverId,
        int fuelRefile,
        decimal cost,
        string details
    );

    Task UpdateTripReportAsync(
        string reportId,
        string driverId,
        string details,
        int fuelRefile,
        decimal cost
    );
    Task DeleteTripReportAsync(string reportId, string managerId);

    Task<IReadOnlyList<TripReport>> GetAllTripReportsAsync(TripReportSpecParams specParams);

    Task<TripReport> GetTripReportByIdAsync(string id);

    Task<IReadOnlyList<TripReport>> GetAllTripReportsForUserAsync(TripReportSpecParams specParams);
}
