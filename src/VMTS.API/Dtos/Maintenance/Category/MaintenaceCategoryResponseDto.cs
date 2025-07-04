using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Category
{
    public class MaintenaceCategoryResponseDto
    {
        public string Id { get; set; } = default!;
        public MaintenanceCategory Categorty { get; set; }
        public string Description { get; set; } = default!;
    }
}
