using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Trip;

public enum TripReportStatus
{
    [EnumMember(Value = "Reported")]
    Reported,

    [EnumMember(Value = "Closed")]
    Closed,
}
