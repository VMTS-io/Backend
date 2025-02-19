namespace VMTS.Core.Entities.Maintenace;

public class MaintenaceReport : BaseEntity
{
    public string Description { get; set; }

    public decimal Cost { get; set; }

    public DateTime Date { get; set; }

    public Status Status { get; set; }
}