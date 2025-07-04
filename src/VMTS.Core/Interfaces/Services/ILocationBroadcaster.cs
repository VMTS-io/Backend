namespace VMTS.Core.Interfaces.Services;

public interface ILocationBroadcaster
{
    Task BroadcastAsync(
        string tripId,
        double lat,
        double lng,
        double startLng,
        double startLat,
        double destLng,
        double destLat
    );
}
