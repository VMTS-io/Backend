using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Trip;

public enum TripType
{
    [EnumMember(Value = "Business")]
    Business,
    [EnumMember(Value = "Personal")]
    Personal
}
