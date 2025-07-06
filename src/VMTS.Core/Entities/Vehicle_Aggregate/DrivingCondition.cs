using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Vehicle_Aggregate;

public enum DrivingCondition
{
    [EnumMember(Value = "Urban")]
    Urban,

    [EnumMember(Value = "Highway")]
    Highway,

    [EnumMember(Value = "Mixed")]
    Mixed,
}
