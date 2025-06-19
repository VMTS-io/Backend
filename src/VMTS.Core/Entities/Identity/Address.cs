using System.ComponentModel.DataAnnotations.Schema;

namespace VMTS.Core.Entities.Identity;

public class Address
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Street { get; set; } = default!;
    public string Area { get; set; } = default!;
    public string Governorate { get; set; } = default!;
    public string Country { get; set; } = default!;

    public string AppUserId { get; set; } = default!;
}
