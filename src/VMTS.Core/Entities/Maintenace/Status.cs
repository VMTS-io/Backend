using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Maintenace;

public enum MaintenanceStatus
{
    [EnumMember(Value = "Pending")]
    Pending,

    [EnumMember(Value = "InProgress")]
    InProgress,

    [EnumMember(Value = "Completed")]
    Completed,
}

