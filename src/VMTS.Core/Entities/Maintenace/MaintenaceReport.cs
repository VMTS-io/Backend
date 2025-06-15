using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Maintenace;

public class MaintenaceReport : BaseEntity
{
    public string Description { get; set; } = default!;

    public decimal Cost { get; set; }

    public DateTime Date { get; set; }

    public Status Status { get; set; }

    public BusinessUser Mechanic { get; set; } = default!;

    public Vehicle Vehicle { get; set; } = default!;
}

