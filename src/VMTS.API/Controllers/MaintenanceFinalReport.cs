using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.ActionFilters;
using VMTS.API.Dtos.Maintenance.Report.Final;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Specifications.Maintenance.Report.Final;

namespace VMTS.API.Controllers;

[Route("api/Maintenance/Report/Final")]
[Tags("Maintenance/Report/Final")]
public class MaintenanceFinalReportController : BaseApiController
{
    private readonly IMaintenanceFinalReportServices _service;
    private readonly IMapper _mapper;

    public MaintenanceFinalReportController(IMaintenanceFinalReportServices service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    #region Create
    [ServiceFilter<ValidateModelActionFilter<MaintenanceFinalReportRequestDto>>]
    [Authorize(Roles = Roles.Mechanic)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MaintenanceFinalReportRequestDto dto)
    {
        var mechanicId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var report = _mapper.Map<MaintenanceFinalReport>(dto);
        report.MechanicId = mechanicId!;
        await _service.CreateFinalReportAsync(report);
        return Ok(new { StatusCode = StatusCodes.Status200OK });
    }
    #endregion

    #region Update
    [ServiceFilter<ValidateModelActionFilter<MaintenanceFinalReportUpdateDto>>]
    [Authorize(Roles = $"{Roles.Mechanic},{Roles.Manager}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] string id,
        [FromBody] MaintenanceFinalReportUpdateDto dto
    )
    {
        var report = _mapper.Map<MaintenanceFinalReport>(dto);
        report.Id = id;
        await _service.UpdateFinalReportAsync(report);
        return Ok(new { StatusCode = StatusCodes.Status200OK });
    }
    #endregion

    #region Delete
    [Authorize(Roles = $"{Roles.Mechanic},{Roles.Manager}")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _service.DeleteFinalReportAsync(id);
        return Ok(new { StatusCode = StatusCodes.Status200OK });
    }

    #endregion

    #region Get By Id
    [Authorize(Roles = $"{Roles.Mechanic},{Roles.Manager}")]
    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceFinalReportDetailsDto>> GetById([FromRoute] string id)
    {
        var report = await _service.GetFinalReportByIdAsync(id);
        var response = _mapper.Map<MaintenanceFinalReportDetailsDto>(report);
        return Ok(new { data = response, StatusCode = StatusCodes.Status200OK });
    }
    #endregion

    #region GetAll
    [Authorize(Roles = $"{Roles.Mechanic},{Roles.Manager}")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaintenanceFinalReportResponseDto>>> GetAll(
        [FromQuery] MaintenanceFinalReportSpecParams specParams
    )
    {
        var reports = await _service.GetAllFinalReportsAsync(specParams);
        var response = _mapper.Map<IReadOnlyList<MaintenanceFinalReportResponseDto>>(reports);
        return Ok(new { data = response, StatusCode = StatusCodes.Status200OK });
    }
    #endregion

    #region seen

    [HttpPatch("{id}/mark-as-seen")]
    public async Task<ActionResult> MarkAsSeen([FromRoute] string id)
    {
        await _service.UpdateMarkAsSeen(id);
        var status = HttpContext.Response.StatusCode;
        return Ok(new { StatusCode = status });
    }

    #endregion
}
