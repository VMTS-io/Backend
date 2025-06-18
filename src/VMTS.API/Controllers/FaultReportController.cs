using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos;
using VMTS.API.Errors;
using VMTS.Core.Entities.Identity;
using VMTS.Core.ServicesContract;
using VMTS.Core.Specifications.FaultReportSepcification;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VMTS.Core.Helpers;
using VMTS.Service.Services;

namespace VMTS.API.Controllers;

public class FaultReportController : BaseApiController
{
    private readonly IReportService _ireportService;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public FaultReportController(
        IReportService ireportService,
        IMapper mapper,
        UserManager<AppUser> userManager
    )
    {
        _ireportService = ireportService;
        _mapper = mapper;
        _userManager = userManager;
    }

    #region Create

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.Driver)]
    [HttpPost]
    [ProducesResponseType(typeof(FaultReportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FaultReportResponse>> Create(FaultReportRequest request)
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
            request.FaultType,
            request.Cost,
            request.FuelRefile,
            request.Address
        );

        var mappedReport = _mapper.Map<FaultReportResponse>(faultReport);
        return Ok(mappedReport);
    }

    #endregion

    #region Get All Reports
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.Manager)]
    [ProducesResponseType(typeof(FaultReportResponse), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FaultReportResponse>>> GetAll([FromQuery] FaultReportSpecParams specParams)
    {
        var faultReports = await _ireportService.GetAllFaultReportsAsync(specParams);

        var mappedReports = _mapper.Map<IReadOnlyList<FaultReportResponse>>(faultReports);

        return Ok(mappedReports);
    }

    #endregion
    
    #region Get By Id

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.Manager)]
[HttpGet("{id}")]
[ProducesResponseType(typeof(FaultReportResponse), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
public async Task<ActionResult<FaultReportResponse>> GetFaultReportById(string id)
{
    var specParams = new FaultReportSpecParams();
    var FaultReport = await _ireportService.GetFaultReportByIdAsync(id);
    var mappedReport = _mapper.Map<FaultReportResponse>(FaultReport);
    return Ok(mappedReport);
}

#endregion

    #region Update

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.Driver)]
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
            request.FuelRefile);
        
        return NoContent();
    }

    #endregion

    #region Delete

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.Manager)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute]string id)
    {
        var managerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _ireportService.DeleteFaultReportAsync(id, managerId);
        return NoContent();
    }   

#endregion

    #region Get All Reports For User
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Manager},{Roles.Driver}")]
    // [HttpGet("/user/{userId}")]
    // public async Task<ActionResult<IReadOnlyList<FaultReportResponse>>> GetAllForUser(string userId)
    // {
    //     var specParams = new FaultReportSpecParams();
    //     var faultReports = await _ireportService.GetAllFaultReportsForUserAsync(userId,specParams);
    //
    //     var mappedReports = _mapper.Map<IReadOnlyList<FaultReportResponse>>(faultReports);
    //     return Ok(mappedReports);
    // }

    #endregion
    
    #region Get All For Vehicle
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.Manager)]
    // [HttpGet("/vehicle/{vehicleId}")]
    // public async Task<ActionResult<IReadOnlyList<FaultReportResponse>>> GetAllForVehicle(string vehicleId)
    // {
    //     var specParams = new FaultReportSpecParams();
    //     var vehicleReports = await _ireportService.GetAllFaultReportsForVehicleAsync(vehicleId, specParams);
    //     var mappedReports = _mapper.Map<IReadOnlyList<FaultReportResponse>>(vehicleReports);
    //     return Ok(mappedReports);
    // }

    #endregion

}
