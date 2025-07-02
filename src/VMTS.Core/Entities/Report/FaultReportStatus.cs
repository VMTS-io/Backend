using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Report;

public enum FaultReportStatus
{
    [EnumMember(Value = "Reported")]
    Reported,

    [EnumMember(Value = "Under Review")]
    UnderReview,

    [EnumMember(Value = "Resolved")]
    Resolved,

    [EnumMember(Value = "Closed")]
    Closed,
}
