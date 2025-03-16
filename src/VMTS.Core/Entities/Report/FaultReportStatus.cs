using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Report;

public enum FaultReportStatus
{
    [EnumMember(Value = "Reported")]
    Reported,    
    [EnumMember(Value = "UnderReview")]
    UnderReview, 
    [EnumMember(Value = "InProgress")]
    InProgress,
    [EnumMember(Value = "Resolved")]
    Resolved, 
    [EnumMember(Value = "Closed")]
    Closed 
}