namespace VMTS.Core.Entities;

public class BaseEntity
{
    public string Id { get; set; } = new Guid().ToString();
}