namespace VMTS.Core.Entities.Maintenace;

public class MaintenaceRequest : BaseEntity
{
    public string Description { get; set; }

    public DateTime Date { get; set; }

    public Status Status { get; set; }
}