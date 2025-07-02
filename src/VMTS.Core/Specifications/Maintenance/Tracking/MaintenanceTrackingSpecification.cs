using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Specifications.Maintenance.Tracking
{
    public class MaintenanceTrackingSpecification
        : BaseSpecification<Entities.Maintenace.MaintenanceTracking>
    {
        public MaintenanceTrackingSpecification() { }

        public MaintenanceTrackingSpecification(string id)
            : base(mt => mt.Id == id) { }
    }
}
