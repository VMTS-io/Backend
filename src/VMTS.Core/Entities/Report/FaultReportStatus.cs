using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Report;

public enum FaultReportStatus
{
    [EnumMember(Value = "Reported")]
    Reported,

    [EnumMember(Value = "UnderReview")]
    UnderReview,

    [EnumMember(Value = "Resolved")]
    Resolved,

    [EnumMember(Value = "Closed")]
    Closed,
}
