using VMTS.API.Dtos.Maintenance;
using VMTS.API.Dtos.Trip;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleDetailsDto
{
    public string PalletNumber { get; set; }

    public DateTime JoindYear { get; set; }

    public FuelType FuelType { get; set; }

    public int KMDriven { get; set; }

    public VehicleStatus Status { get; set; } = VehicleStatus.Active;

    public VehicleModelDto VehicleModel { get; set; }

    public VehicleCategoryDto VehicleCategory { get; set; }

    public ICollection<TripRequestDto> TripRequests { get; set; } = new HashSet<TripRequestDto>();

    public ICollection<TripReportDto> TripReports { get; set; } = new HashSet<TripReportDto>();

    public ICollection<MaintenanceReportDto> MaintenaceReports { get; set; } =
        new HashSet<MaintenanceReportDto>();
    public ICollection<VehicleMaintenanceRequestDto> MaintenaceRequests { get; set; } =
        new HashSet<VehicleMaintenanceRequestDto>();
}
