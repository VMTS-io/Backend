using VMTS.API.Dtos.Maintenance;
using VMTS.API.Dtos.Trip;
using VMTS.API.Dtos.Vehicles.Model;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleDetailsDto
{
    public string PalletNumber { get; set; } = default!;

    public DateOnly JoindYear { get; set; }

    public FuelType FuelType { get; set; }

    public int KMDriven { get; set; }

    public VehicleStatus Status { get; set; } = VehicleStatus.Active;

    public VehicleModelDto VehicleModel { get; set; } = default!;

    public DateOnly? ModelYear { get; set; }

    // public VehicleCategoryDto VehicleCategory { get; set; }

    public ICollection<TripRequestDto> TripRequests { get; set; } = [];

    public ICollection<TripReportDto> TripReports { get; set; } = [];

    public ICollection<MaintenanceReportDto> MaintenaceReports { get; set; } = [];

    public ICollection<VehicleMaintenanceRequestDto> MaintenaceRequests { get; set; } = [];
}
