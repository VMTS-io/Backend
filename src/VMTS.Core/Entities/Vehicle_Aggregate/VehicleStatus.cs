using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Vehicle_Aggregate;

public enum VehicleStatus
{
    [EnumMember(Value = "Active")]
    Active,

    [EnumMember(Value = "Inactive")]
    Inactive,

    [EnumMember(Value = "UnderMaintenance")]
    UnderMaintenance,

    [EnumMember(Value = "Retired")]
    Retired,
}

