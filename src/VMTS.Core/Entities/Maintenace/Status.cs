using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Maintenace;

public enum MaintenanceStatus
{
    [EnumMember(Value = "Pending")]
    Pending,

    [EnumMember(Value = "In Progress")]
    InProgress,

    [EnumMember(Value = "Completed")]
    Completed,
}
