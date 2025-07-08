namespace VMTS.Core.Entities.Ai;

public class AiEndpointConfig : BaseEntity
{
    public string Name { get; set; }
    public string Url { get; set; } = default!;
}
