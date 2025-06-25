namespace VMTS.API.Dtos.Maintenance.Report.Final
{
    public class MaintenanceFinalReportSummaryDto
    {
        public string Id { get; set; } = default!;
        public string Notes { get; set; } = default!;
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }
        public DateOnly FinishDate { get; set; }
        public string MaintenanceCategory { get; set; } = default!;
    }
}

