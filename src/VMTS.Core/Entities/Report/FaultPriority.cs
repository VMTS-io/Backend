using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Report;

public enum FaultPriority
{
    [EnumMember(Value = "Low")]
    Low,

    [EnumMember(Value = "Medium")]
    Medium,

    [EnumMember(Value = "High")]
    High,
}
