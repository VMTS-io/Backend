using Microsoft.AspNetCore.SignalR;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Hubs;

public class SignalRLocationBroadcaster : ILocationBroadcaster
{
    private readonly IHubContext<LocationHub> _hub;

    public SignalRLocationBroadcaster(IHubContext<LocationHub> hub)
    {
        _hub = hub;
    }

    public Task BroadcastAsync(string tripId, double lat, double lng)
    {
        return _hub.Clients.All.SendAsync("ReceiveLocation", tripId, lat, lng);
    }
}
