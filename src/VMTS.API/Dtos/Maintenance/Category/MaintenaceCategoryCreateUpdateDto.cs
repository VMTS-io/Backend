using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Category;

public class MaintenaceCategoryCreateUpdateDto
{
    public Categorty Categorty { get; set; }
    public string Description { get; set; } = default!;
}
