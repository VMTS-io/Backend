using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Category;

public class MaintenaceCategoryCreateUpdateDto
{
    public MaintenanceCategory Categorty { get; set; }
    public string Description { get; set; } = default!;
}
