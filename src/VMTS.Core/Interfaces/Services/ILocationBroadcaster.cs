namespace VMTS.Core.Interfaces.Services;

public interface ILocationBroadcaster
{
    Task BroadcastAsync(string tripId, double lat, double lng);
}
