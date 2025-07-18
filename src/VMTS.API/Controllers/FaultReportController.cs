﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.ActionFilters;
using VMTS.API.Dtos;
using VMTS.API.Errors;
using VMTS.Core.Helpers;
using VMTS.Core.ServicesContract;
using VMTS.Core.Specifications.FaultReportSepcification;

namespace VMTS.API.Controllers;

[Route("api/Trip/Report/Fault")]
public class FaultReportController : BaseApiController
{
    private readonly IFaultReportService _ireportService;
    private readonly IMapper _mapper;

    public FaultReportController(IFaultReportService ireportService, IMapper mapper)
    {
        _ireportService = ireportService;
        _mapper = mapper;
    }

    #region Create

    [Authorize(Roles = Roles.Driver)]
    [HttpPost]
    [ServiceFilter(typeof(ValidateModelActionFilter<FaultReportRequest>))]
    [ProducesResponseType(typeof(FaultReportSingleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FaultReportSingleResponse>> Create(FaultReportRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Unauthorized",
                detail: "User ID not found in token claims. Please login again."
            );
        }

        var faultReport = await _ireportService.CreateFaultReportAsync(
            userId,
            request.Details,
            request.Cost,
            request.FuelRefile,
            request.Address
        );

        var status = HttpContext.Response.StatusCode;
        var mappedReport = _mapper.Map<FaultReportResponse>(faultReport);
        return Ok(
            new FaultReportSingleResponse { StatusCode = status, FaultReport = mappedReport }
        );
    }

    #endregion

    #region Get All Reports (Manager)

    [Authorize(Roles = Roles.Manager)]
    [HttpGet]
    [ProducesResponseType(typeof(FaultReportListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<FaultReportListResponse>> GetAll(
        [FromQuery] FaultReportSpecParams specParams
    )
    {
        var faultReports = await _ireportService.GetAllFaultReportsAsync(specParams);
        var status = HttpContext.Response.StatusCode;
        var mappedReports = _mapper.Map<List<FaultReportResponse>>(faultReports);

        return Ok(
            new FaultReportListResponse { StatusCode = status, FaultReports = mappedReports }
        );
    }

    #endregion

    #region Get By Id

    [Authorize(Roles = $"{Roles.Driver},{Roles.Manager}")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FaultReportSingleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FaultReportSingleResponse>> GetFaultReportById(string id)
    {
        var faultReport = await _ireportService.GetFaultReportByIdAsync(id);
        var status = HttpContext.Response.StatusCode;
        var mappedReport = _mapper.Map<FaultReportResponse>(faultReport);

        return Ok(
            new FaultReportSingleResponse { StatusCode = status, FaultReport = mappedReport }
        );
    }

    #endregion

    #region Update

    [Authorize(Roles = Roles.Driver)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromRoute] string id, UpdateFaultReportRequest request)
    {
        var driverId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await _ireportService.UpdateFaultReportAsync(
            id,
            driverId,
            request.Details,
            request.FaultAddress,
            request.Cost,
            request.FuelRefile
        );

        return NoContent();
    }

    #endregion

    #region Delete

    [Authorize(Roles = Roles.Manager)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var managerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _ireportService.DeleteFaultReportAsync(id, managerId);
        return NoContent();
    }

    #endregion

    #region Get All Reports For Current Driver

    [Authorize(Roles = Roles.Driver)]
    [HttpGet("/me/reports/faults")]
    [ProducesResponseType(typeof(FaultReportListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FaultReportListResponse>> GetAllForCurrentDriver(
        [FromQuery] FaultReportSpecParams specParams
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

        specParams.DriverId = driverId;

        var faultReports = await _ireportService.GetAllFaultReportsForUserAsync(specParams);
        var status = HttpContext.Response.StatusCode;
        var mappedReports = _mapper.Map<List<FaultReportResponse>>(faultReports);

        return Ok(
            new FaultReportListResponse { StatusCode = status, FaultReports = mappedReports }
        );
    }

    #endregion

    #region seen

    [HttpPatch("Report/Fault/{id}/mark-as-seen")]
    public async Task<ActionResult> MarkAsSeen([FromRoute] string id)
    {
        await _ireportService.UpdateMarkAsSeen(id);
        var status = HttpContext.Response.StatusCode;
        return Ok(new { StatusCode = status });
    }

    #endregion
}
