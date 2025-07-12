using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos;
using VMTS.API.Dtos.Trip;
using VMTS.API.Errors;
using VMTS.Core.Entities.Trip;
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
    [ProducesResponseType(typeof(TripRequestSingleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripRequestSingleResponse>> CreateTripRequestAsync(
        TripRequestDto request
    )
    {
        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(managerId))
        {
            return Problem(
                statusCode: 401,
                title: "Unauthorized",
                detail: "Invalid manager authentication"
            );
        }

        var tripRequest = await _requestService.CreateTripRequestAsync(
            managerId,
            request.DriverId,
            request.VehicleId,
            request.TripType,
            request.Date,
            request.Details,
            request.PickupLocation,
            request.Destination,
            request.IsDaily
        );

        var status = HttpContext.Response.StatusCode;
        var mapped = _mapper.Map<TripRequestResponse>(tripRequest);

        return Ok(new TripRequestSingleResponse { StatusCode = status, TripRequest = mapped });
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
        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _requestService.UpdateTripRequestAsync(
            id,
            managerId,
            request.DriverId,
            request.VehicleId,
            request.Details,
            request.Date,
            request.TripType,
            request.Status,
            request.PickupLocation,
            request.Destination
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
        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _requestService.DeleteTripRequestAsync(id, managerId);
        return NoContent();
    }

    #endregion

    #region Get By Id

    [Authorize(Roles = $"{Roles.Manager},{Roles.Driver}")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TripRequestSingleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripRequestSingleResponse>> GetById(string id)
    {
        var tripRequest = await _requestService.GetTripRequestByIdAsync(id);

        var status = HttpContext.Response.StatusCode;
        var mapped = _mapper.Map<TripRequestResponse>(tripRequest);

        return Ok(new TripRequestSingleResponse { StatusCode = status, TripRequest = mapped });
    }

    #endregion

    #region Get All

    [Authorize(Roles = "Manager")]
    [HttpGet]
    [ProducesResponseType(typeof(TripRequestListResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<TripRequestListResponse>> GetAll(
        [FromQuery] TripRequestSpecParams specParams
    )
    {
        var tripRequests = await _requestService.GetAllTripRequestsAsync(specParams);

        var status = HttpContext.Response.StatusCode;
        var mapped = _mapper.Map<List<TripRequestResponse>>(tripRequests);

        return Ok(new TripRequestListResponse { StatusCode = status, TripRequests = mapped });
    }

    #endregion

    #region Get All for Driver

    [Authorize(Roles = Roles.Driver)]
    [HttpGet("/me/trips")]
    [ProducesResponseType(typeof(TripRequestListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripRequestListResponse>> GetAllForCurrentDriver()
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

        var status = HttpContext.Response.StatusCode;
        var mapped = _mapper.Map<List<TripRequestResponse>>(trips);

        return Ok(new TripRequestListResponse { StatusCode = status, TripRequests = mapped });
    }

    #endregion

    #region update status

    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateStatus([FromRoute] string id)
    {
        await _requestService.UpdateTripRequestStatusAsync(id);
        var status = HttpContext.Response.StatusCode;
        return Ok(new { StatusCode = status });
        ;
    }

    #endregion

    [HttpPatch("Cancel-OneTime/{id}")]
    public async Task<ActionResult> UpdateOneTimeStatus([FromRoute] string id)
    {
        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _requestService.RemoveOneTimeTripAsync(id, managerId);
        ;
        var status = HttpContext.Response.StatusCode;
        return Ok(new { StatusCode = status });
        ;
    }

    [HttpPatch("Cancel-Daily/{id}")]
    public async Task<ActionResult> UpdateDailyStatus([FromRoute] string id)
    {
        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _requestService.RemoveDailyTripAsync(id, managerId);
        ;
        var status = HttpContext.Response.StatusCode;
        return Ok(new { StatusCode = status });
        ;
    }
}
