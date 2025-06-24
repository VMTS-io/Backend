namespace VMTS.API.Dtos.Maintenance.Report
{
    public class MaintenanceReportPartResponseDto
    {
        public PartForMaintenanceReportDto Part { get; set; } = default!;
        public int Quantity { get; set; }
    }
}
