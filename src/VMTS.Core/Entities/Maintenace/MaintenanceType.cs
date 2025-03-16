using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Maintenace;

public enum MaintenanceType
{
    [EnumMember(Value = "Mechanical")]
    Mechanical,   // Engine, transmission, brakes, etc.
    [EnumMember(Value = "Electrical")]
    Electrical,   // Battery, lights, sensors, wiring, etc.
    [EnumMember(Value = "Tire")]
    Tire,         // Punctures, blowouts, low pressure
    [EnumMember(Value = "Fuel")]
    Fuel,         // Fuel leaks, low fuel issues
    [EnumMember(Value = "Cooling")]
    Cooling,      // Radiator, coolant leaks, overheating
    [EnumMember(Value = "Brake")]
    Brake,        // Brake failure, worn-out pads
    [EnumMember(Value = "Suspension")]
    Suspension,   // Shock absorbers, wheel alignment issues
    [EnumMember(Value = "Transmission")]
    Transmission, // Gear shifting, clutch problems
    [EnumMember(Value = "Accident")]
    Accident,     // Collision, crash, road hazard damage
    [EnumMember(Value = "Other")]
    Other         // Any other unclassified issues
}