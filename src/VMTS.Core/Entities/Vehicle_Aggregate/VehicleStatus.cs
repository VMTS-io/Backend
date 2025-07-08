using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Vehicle_Aggregate;

public enum VehicleStatus
{
    [EnumMember(Value = "Available")]
    Available, // The vehicle is functional and not currently in use

    [EnumMember(Value = "On a Trip")]
    OnTrip, // Currently on a trip or reserved

    [EnumMember(Value = "Under Maintenance")]
    UnderMaintenance, // Unavailable due to ongoing repairs or checks

    [EnumMember(Value = "Out of Service")]
    OutOfService, // Broken or unusable, but not retired yet

    [EnumMember(Value = "Retired")]
    Retired // Permanently decommissioned
    ,
}
