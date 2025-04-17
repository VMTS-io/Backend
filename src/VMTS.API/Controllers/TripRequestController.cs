using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos;
using VMTS.API.Errors;
using VMTS.Core.ServicesContract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VMTS.API.Controllers;

public class TripRequestController : BaseApiController
{
    private readonly ITripRequestService _requestService;

    public TripRequestController(ITripRequestService requestService)
    {
        _requestService = requestService;
    }
    

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
    [HttpPost("create")]
    public async Task<ActionResult<TripRequestResponse>> CreateTripRequestAsync(TripRequestDto request)
    {
        var manager =User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(manager))
            return Unauthorized(new ApiResponse(401));
        
        var tripRequest = await _requestService.CreateTripRequestAsync(
            manager,
            request.DriverId,
            request.VehicleId,
            request.TripType,
            request.Details,
            request.Destination
        );

        if (tripRequest is null)
            return BadRequest(new ApiResponse(400, "Failed to create trip request"));

        var response = new TripRequestResponse
        {
            TripType = tripRequest.Type,
            Details = tripRequest.Details,
            Destination = tripRequest.Destination,
            Date = tripRequest.Date,
            TripStatus = tripRequest.Status,
            DriverId = tripRequest.DriverId,
            ManagerId = tripRequest.ManagerId,
            VehicleId = tripRequest.Vehicle.Id
        };

        return Ok(response);
    }
    
}