using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Trip;

public enum TripStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Approved")]
    Approved
}