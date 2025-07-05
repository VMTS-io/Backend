using VMTS.Core.ServicesContract;

namespace VMTS.Repository.Data.Jobs;

public class AssignDailyTrip
{
    private readonly ITripRequestService _tripRequestService;

    public AssignDailyTrip(ITripRequestService tripRequestService)
    {
        _tripRequestService = tripRequestService;
    }

    public async Task RunAssignDailyTrip()
    {
        await _tripRequestService.GenerateDailyTripsFromTemplatesAsync();
    }
}
