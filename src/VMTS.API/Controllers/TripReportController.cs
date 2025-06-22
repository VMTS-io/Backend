using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.TripReport;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Specifications;

namespace VMTS.API.Controllers;

public class TripReportController : BaseApiController
{
    private readonly ITripReportService _tripReportService;
    private readonly IMapper _mapper;

    public TripReportController(ITripReportService tripReportService, IMapper mapper)
    {
        _tripReportService = tripReportService;
        _mapper = mapper;
    }

    #region create

    [Authorize(Roles = Roles.Driver)]
    [HttpPost]
    [ProducesResponseType(typeof(TripReportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TripReportResponse>> Create(TripReportRequest request)
    {
        var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(driverId))
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Unauthorized",
                detail: "User ID not found in token claims. Please login again."
            );
        }

        var tripReport = await _tripReportService.CreateTripReportAsync(
            driverId,
            request.FuelRefile,
            request.Cost,
            request.Details
        );

        var mappedReport = _mapper.Map<TripReportResponse>(tripReport);
        return Ok(mappedReport);
    }

    #endregion

    #region update

    [Authorize(Roles = Roles.Driver)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(
        [FromRoute] string id,
        TripReportUpdateRequest updateRequest
    )
    {
        var driverId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _tripReportService.UpdateTripReportAsync(
            id,
            driverId,
            updateRequest.Details,
            updateRequest.FuelRefile,
            updateRequest.Cost
        );
        return NoContent();
    }

    #endregion

    #region delete

    [Authorize(Roles = Roles.Manager)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var managerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _tripReportService.DeleteTripReportAsync(id, managerId);
        return NoContent();
    }

    #endregion

    #region get all

    [Authorize(Roles = Roles.Manager)]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TripReportResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<TripReportResponse>>> GetAll(
        [FromQuery] TripReportSpecParams specParams
    )
    {
        var tripReports = await _tripReportService.GetAllTripReportsAsync(specParams);
        var mappedReports = _mapper.Map<IReadOnlyList<TripReportResponse>>(tripReports);
        return Ok(mappedReports);
    }

    #endregion

    #region get by id

    [Authorize(Roles = $"{Roles.Driver},{Roles.Manager}")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TripReportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripReportResponse>> GetById(string id)
    {
        var tripReport = await _tripReportService.GetTripReportByIdAsync(id);
        var mappedReport = _mapper.Map<TripReportResponse>(tripReport);
        return Ok(mappedReport);
    }

    #endregion

    #region Get All Reports For User

    [Authorize(Roles = Roles.Driver)]
    [HttpGet("/me/reports/regular")]
    [ProducesResponseType(typeof(IReadOnlyList<TripReportResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<TripReportResponse>>> GetAllForCurrentDriver(
        [FromQuery] TripReportSpecParams specParams
    )
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

        specParams.DriverId = driverId; // Force driver's own data only

        var tripReports = await _tripReportService.GetAllTripReportsForUserAsync(specParams);
        var mappedReports = _mapper.Map<IReadOnlyList<TripReportResponse>>(tripReports);
        return Ok(mappedReports);
    }

    #endregion
}
