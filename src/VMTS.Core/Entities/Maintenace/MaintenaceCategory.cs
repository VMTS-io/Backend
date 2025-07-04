namespace VMTS.Core.Entities.Maintenace;

public class MaintenaceCategories : BaseEntity
{
    public MaintenanceCategory Categorty { get; set; }

    public string Description { get; set; } = default!;
}
