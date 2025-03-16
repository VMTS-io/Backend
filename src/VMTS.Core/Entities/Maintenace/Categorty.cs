using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Maintenace;

public enum Categorty
{
    [EnumMember(Value = "Faults")]
    Faults,
    [EnumMember(Value = "Millage")]
    Millage,
    [EnumMember(Value = "TimeBased")]
    TimeBased
}