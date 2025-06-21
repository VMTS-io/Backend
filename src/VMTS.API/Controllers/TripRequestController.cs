using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos;
using VMTS.API.Dtos.Trip;
using VMTS.API.Errors;
using VMTS.Core.Helpers;
using VMTS.Core.ServicesContract;
using VMTS.Core.Specifications.TripRequestSpecification;

namespace VMTS.API.Controllers;

public class TripRequestController : BaseApiController
{
    private readonly ITripRequestService _requestService;
    private readonly IMapper _mapper;

    public TripRequestController(ITripRequestService requestService, IMapper mapper)
    {
        _requestService = requestService;
        _mapper = mapper;
    }

    #region Create

    [Authorize(Roles = "Manager")]
    [HttpPost]
    [ProducesResponseType(typeof(TripRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripRequestResponse>> CreateTripRequestAsync(
        TripRequestDto request
    )
    {
        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(managerId))
            return Problem(
                statusCode: 401,
                title: "Unauthorized",
                detail: "Invalid manager authentication"
            );

        var tripRequest = await _requestService.CreateTripRequestAsync(
            managerId,
            request.DriverId,
            request.VehicleId,
            request.TripType,
            request.Date,
            request.Details,
            request.Destination
        );

        var mappedTripRequest = _mapper.Map<TripRequestResponse>(tripRequest);
        return Ok(mappedTripRequest);
    }

    #endregion

    #region Update

    [Authorize(Roles = "Manager")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(
        [FromRoute] string id,
        [FromBody] TripRequestUpdateDto request
    )
    {
        var managerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _requestService.UpdateTripRequestAsync(
            id,
            managerId,
            request.DriverId,
            request.VehicleId,
            request.Destination,
            request.Details,
            request.Date,
            request.TripType,
            request.Status
        );

        return NoContent();
    }

    #endregion

    #region Delete

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var managerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _requestService.DeleteTripRequestAsync(id, managerId);
        return NoContent();
    }

    #endregion

    #region Get By Id

    [Authorize(Roles = $"{Roles.Manager},{Roles.Driver}")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TripRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripRequestResponse>> GetById(string id)
    {
        var tripRequest = await _requestService.GetTripRequestByIdAsync(id);
        var mappedTripRequest = _mapper.Map<TripRequestResponse>(tripRequest);
        return Ok(mappedTripRequest);
    }

    #endregion

    #region Get All

    [Authorize(Roles = "Manager")]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TripRequestResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TripRequestResponse>>> GetAll(
        [FromQuery] TripRequestSpecParams specParams
    )
    {
        var tripRequests = await _requestService.GetAllTripRequestsAsync(specParams);
        var mappedTripRequests = _mapper.Map<IReadOnlyList<TripRequestResponse>>(tripRequests);
        return Ok(mappedTripRequests);
    }

    #endregion

    #region Get All for Driver
    [Authorize(Roles = Roles.Driver)]
    [HttpGet("/me/trips")]
    [ProducesResponseType(typeof(IReadOnlyList<TripRequestResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<TripRequestResponse>>> GetAllForCurrentDriver()
    {
        var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(driverId))
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Unauthorized",
                detail: "User ID not found in token claims. Please login again."
            );
        }

        var specParams = new TripRequestSpecParams { DriverId = driverId };

        var trips = await _requestService.GetAllTripsForUserAsync(specParams);
        var mappedTrips = _mapper.Map<IReadOnlyList<TripRequestResponse>>(trips);
        return Ok(mappedTrips);
    }

    #endregion
}
