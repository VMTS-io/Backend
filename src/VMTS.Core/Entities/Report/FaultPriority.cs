using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Report;

public enum FaultPriority
{
    [EnumMember(Value = "Low")]
    Low = 1,

    [EnumMember(Value = "Medium")]
    Medium = 2,

    [EnumMember(Value = "High")]
    High = 3,
}
