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

    #region Create

    [Authorize(Roles = Roles.Driver)]
    [HttpPost]
    [ProducesResponseType(typeof(TripReportSingleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TripReportSingleResponse>> Create(TripReportRequest request)
    {
        var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(driverId))
        {
            return Problem(
                "User ID not found in token claims. Please login again.",
                "Unauthorized",
                401
            );
        }

        var tripReport = await _tripReportService.CreateTripReportAsync(
            driverId,
            request.FuelRefile,
            request.Cost,
            request.Details
        );

        var mapped = _mapper.Map<TripReportResponse>(tripReport);
        var status = HttpContext.Response.StatusCode;
        return Ok(new TripReportSingleResponse { StatusCode = status, TripReport = mapped });
    }

    #endregion

    #region Update

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

    #region Delete

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

    #region Get All (Manager)

    [Authorize(Roles = Roles.Manager)]
    [HttpGet]
    [ProducesResponseType(typeof(TripReportListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripReportListResponse>> GetAll(
        [FromQuery] TripReportSpecParams specParams
    )
    {
        var tripReports = await _tripReportService.GetAllTripReportsAsync(specParams);
        var mapped = _mapper.Map<List<TripReportResponse>>(tripReports);
        var status = HttpContext.Response.StatusCode;

        return Ok(new TripReportListResponse { StatusCode = status, TripReports = mapped });
    }

    #endregion

    #region Get By Id

    [Authorize(Roles = $"{Roles.Driver},{Roles.Manager}")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TripReportSingleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripReportSingleResponse>> GetById(string id)
    {
        var tripReport = await _tripReportService.GetTripReportByIdAsync(id);
        var mapped = _mapper.Map<TripReportResponse>(tripReport);
        var status = HttpContext.Response.StatusCode;

        return Ok(new TripReportSingleResponse { StatusCode = status, TripReport = mapped });
    }

    #endregion

    #region Get All for Current Driver

    [Authorize(Roles = Roles.Driver)]
    [HttpGet("/me/reports/regular")]
    [ProducesResponseType(typeof(TripReportListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TripReportListResponse>> GetAllForCurrentDriver(
        [FromQuery] TripReportSpecParams specParams
    )
    {
        var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(driverId))
        {
            return Problem(
                "User ID not found in token claims. Please login again.",
                "Unauthorized",
                401
            );
        }

        specParams.DriverId = driverId;

        var tripReports = await _tripReportService.GetAllTripReportsForUserAsync(specParams);
        var mapped = _mapper.Map<List<TripReportResponse>>(tripReports);
        var status = HttpContext.Response.StatusCode;

        return Ok(new TripReportListResponse { StatusCode = status, TripReports = mapped });
    }

    #endregion

    #region seen

    [HttpPatch("Report/Regular/{id}/mark-as-seen")]
    public async Task<ActionResult> MarkAsSeen([FromRoute] string id)
    {
        await _tripReportService.UpdateMarkAsSeen(id);
        var status = HttpContext.Response.StatusCode;
        return Ok(new { StatusCode = status });
    }

    #endregion
}
