using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Vehicle_Aggregate;

public enum VehicleStatus
{
    [EnumMember(Value = "Active")]
    Active,
    [EnumMember(Value = "Free")]
    InActive,
    [EnumMember(Value = "UnderMaintenance")]
    UnderMaintenance
}