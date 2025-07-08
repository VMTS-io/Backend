using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Vehicle_Aggregate;

public enum PartCondition
{
    [EnumMember(Value = "New")]
    New,

    [EnumMember(Value = "Good")]
    Good,

    [EnumMember(Value = "Weak")]
    Weak,
}
