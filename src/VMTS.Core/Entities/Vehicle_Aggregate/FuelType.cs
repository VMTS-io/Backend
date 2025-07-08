using System.Runtime.Serialization;

namespace VMTS.Core.Entities.Vehicle_Aggregate;

public enum FuelType
{
    [EnumMember(Value = "Petrol")]
    Petrol,

    [EnumMember(Value = "Diesel")]
    Diesel,

    [EnumMember(Value = "Electric")]
    Electric,
}
