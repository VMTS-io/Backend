using System.Text.Json;
using StackExchange.Redis;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Interfaces.Services;

namespace VMTS.Service.Services;

public class TripLocationService : ITripLocationService
{
    private readonly IDatabase _redis;
    private readonly ILocationBroadcaster _broadcaster;

    public TripLocationService(IConnectionMultiplexer redis, ILocationBroadcaster broadcaster)
    {
        _redis = redis.GetDatabase();
        _broadcaster = broadcaster;
    }

    public async Task SetLocationAsync(TripLocation model)
    {
        var key = $"trip:{model.TripId}";
        var json = JsonSerializer.Serialize(model);

        await _redis.StringSetAsync(key, json, TimeSpan.FromHours(5));

        await _broadcaster.BroadcastAsync(
            model.TripId,
            model.Lat,
            model.Lng,
            model.StartLat,
            model.StartLng,
            model.DestinationLat,
            model.DestinationLng
        );
    }

    public async Task<TripLocation?> GetLocationAsync(string tripId)
    {
        var key = $"trip:{tripId}";
        var data = await _redis.StringGetAsync(key);

        if (data.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<TripLocation>(data!);
    }
}
