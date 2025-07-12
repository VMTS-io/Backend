using Microsoft.AspNetCore.Mvc;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Controllers;

[Route("api/Vehicle/Odometer")]
public class OdometerReading : BaseApiController
{
    private readonly IOdometerService _ododmeterService;

    public OdometerReading(IOdometerService ododmeterService)
    {
        _ododmeterService = ododmeterService;
    }

    [HttpPost("reading")]
    public async Task<IActionResult> UpdateReadingAsync(
        [FromQuery] string vehicleId,
        [FromForm] IFormFile odometerImage
    )
    {
        if (odometerImage == null || odometerImage.Length == 0)
        {
            return BadRequest("Odometer image is required.");
        }

        await using var stream = odometerImage.OpenReadStream();

        var updatedReading = await _ododmeterService.ProcessAndUpdateOdometerReadingAsync(
            vehicleId,
            stream,
            odometerImage.ContentType,
            odometerImage.FileName
        );

        return Ok(new { VehicleId = vehicleId, UpdatedReading = updatedReading });
    }
}
