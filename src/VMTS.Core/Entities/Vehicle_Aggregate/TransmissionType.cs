using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Vehicle_Aggregate;

public enum TransmissionType
{
    [EnumMember(Value = "Manual")]
    Manual,

    [EnumMember(Value = "Automatic")]
    Automatic,
}
