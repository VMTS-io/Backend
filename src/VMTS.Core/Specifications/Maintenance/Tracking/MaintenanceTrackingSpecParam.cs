namespace VMTS.Core.Specifications.Maintenance.Tracking
{
    public class MaintenanceTrackingSpecParam
    {
        public string? VehicleId { get; set; } = default!;
        public string? PartId { get; set; } = default!;

        public DateTime? LastChangedDate { get; set; }
        public int? KMAtLastChange { get; set; }

        public DateTime? NextChangeDate { get; set; } // computed
        public int? NextChangeKM { get; set; } // computed

        public bool? IsDue { get; set; } = false; // updated by a background job or on query
        public bool? IsAlmostDue { get; set; } = false; // optional for pre-warning
        private int pageSize = 5;
        private const int maxPageSize = 10;
        public int PageSize
        {
            set => pageSize = value > maxPageSize ? maxPageSize : value;
            get => pageSize;
        }
        public int PageIndex { set; get; } = 1;
    }
}

