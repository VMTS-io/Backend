namespace VMTS.Core.Entities.Parts;

public class DuePart
{
    public string PartId { get; set; }
    public string PartName { get; set; }

    public bool IsDue { get; set; } // "Due" or "AlmostDue"

    public bool IsAlmostDue { get; set; } // "Due" or "AlmostDue"
    public int LastReplacedAtKm { get; set; }
    public int NextChangeKm { get; set; }

    public DateTime NextChangeDate { get; set; }
    public int CurrentKm { get; set; }
}
