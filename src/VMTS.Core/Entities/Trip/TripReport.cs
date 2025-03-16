using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Trip;

public class TripReport : BaseEntity
{
    public string Duration { get; set; }

    public string Details { get; set; }

    public DateTime Date { get; set; }

    public decimal FuelCost { get; set; }

    // public bool IsFault {get; set; } = false;
    //
    // public FaultTypes FaultType { get; set; }
    
    public BusinessUser Driver { get; set; }
    
    public Vehicle Vehicle { get; set; }

}