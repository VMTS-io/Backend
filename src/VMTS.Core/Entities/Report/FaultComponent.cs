using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Report;

public enum FaultComponent
{
    [EnumMember(Value = "Engine")]
    Engine,
    [EnumMember(Value = "Transmission")]
    Transmission,
    [EnumMember(Value = "Clutch")]
    Clutch,
    [EnumMember(Value = "Brakes")]
    Brakes,
    [EnumMember(Value = "BrakePads")]
    BrakePads,
    [EnumMember(Value = "ABS")]
    ABS,
    [EnumMember(Value = "Battery")]
    Battery,
    [EnumMember(Value = "Alternator")]
    Alternator,
    [EnumMember(Value = "StarterMotor")]
    StarterMotor,
    [EnumMember(Value = "Wiring")]
    Wiring,
    [EnumMember(Value = "Radiator")]
    Radiator,
    [EnumMember(Value = "CoolantSystem")]
    CoolantSystem,
    [EnumMember(Value = "FuelPump")]
    FuelPump,
    [EnumMember(Value = "FuelInjector")]
    FuelInjector,
    [EnumMember(Value = "FuelTank")]
    FuelTank,
    [EnumMember(Value = "Tires")]
    Tires,
    [EnumMember(Value = "WheelAlignment")]
    WheelAlignment,
    [EnumMember(Value = "Headlights")]
    Headlights,
    [EnumMember(Value = "Taillights")]
    Taillights,
    [EnumMember(Value = "TurnSignal")]
    TurnSignal,
    [EnumMember(Value = "Indicators")]
    Indicators,
    [EnumMember(Value = "Wipers")]
    Wipers,
    [EnumMember(Value = "Airbags")]
    Airbags,
    [EnumMember(Value = "SeatBelts")]
    SeatBelts,
    [EnumMember(Value = "Other")]
    Other
}