using System.Text.Json.Serialization;

namespace VMTS.API.Dtos;

public class BusinessUserGetAllResponse
{
    // common
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Role { get; set; }
}
