namespace VMTS.Core.Specifications.Maintenance.Report.Initial;

public class MaintenanceIntialReportSpecParams
{
    public string? MechanicId { get; set; }
    public string? VehicleId { get; set; }
    public string? MaintenanceRequestId { get; set; }
    public DateTime? Date { get; set; }
    public string? Sort { get; set; }
    public string? Search { get; set; }
}
