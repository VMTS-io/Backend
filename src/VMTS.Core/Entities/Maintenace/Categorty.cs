using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Maintenace;

public enum MaintenanceCategory
{
    [EnumMember(Value = "Faults")]
    Faults,

    [EnumMember(Value = "Regular")]
    Regular,

    [EnumMember(Value = "Ai Predicted")]
    AiPredicted,
    //
    // [EnumMember(Value = "Time Based")]
    // TimeBased,
}
