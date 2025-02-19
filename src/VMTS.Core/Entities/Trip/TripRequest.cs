using VMTS.Core.Entities.Maintenace;
using Type = VMTS.Core.Entities.Maintenace.Type;

namespace VMTS.Core.Entities.Trip;

public class TripRequest : BaseEntity
{
    public TripType Type { get; set; }
    public string Details { get; set; }
    public DateTime Date { get; set; }
    public TripStatus Status { get; set; }
}