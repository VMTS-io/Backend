using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Maintenance.Report.Initial;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Specifications.Maintenance.Report.Initial;

namespace VMTS.API.Controllers;

[Route("api/Maintenance/Report/Initial")]
[Tags("Maintenance/Report/Initial")]
public class MaintenanceInitialReportController : BaseApiController
{
    private readonly IMaintenanceInitialReportServices _service;
    private readonly IMapper _mapper;

    public MaintenanceInitialReportController(
        IMaintenanceInitialReportServices service,
        IMapper mapper
    )
    {
        _service = service;
        _mapper = mapper;
    }

    #region Create
    [Authorize(Roles = Roles.Mechanic)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MaintenanceInitialReportRequestDto dto)
    {
        var mechanicId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var report = _mapper.Map<MaintenanceInitialReport>(dto);
        report.MechanicId = mechanicId!;
        await _service.CreateInitialReportAsync(report);
        return Ok();
    }
    #endregion

    #region Update
    [Authorize(Roles = Roles.Mechanic)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] string id,
        [FromBody] MaintenanceInitialReportUpdateDto dto
    )
    {
        var report = _mapper.Map<MaintenanceInitialReport>(dto);
        report.Id = id;
        await _service.UpdateInitialReportAsync(report);
        return NoContent();
    }
    #endregion

    #region Delete
    [Authorize(Roles = $"{Roles.Mechanic},{Roles.Manager}")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _service.DeleteInitialReportAsync(id);
        return NoContent();
    }
    #endregion

    #region Get by id
    [Authorize(Roles = $"{Roles.Mechanic},{Roles.Manager}")]
    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceInitialReportDetailsDto>> GetById(string id)
    {
        var report = await _service.GetInitialReportByIdAsync(id);
        var response = _mapper.Map<MaintenanceInitialReportDetailsDto>(report);
        return Ok(response);
    }
    #endregion

    #region Get All
    [Authorize(Roles = $"{Roles.Mechanic},{Roles.Manager}")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaintenanceInitialReportResponseDto>>> GetAll(
        [FromQuery] MaintenanceIntialReportSpecParams specParams
    )
    {
        var reports = await _service.GetAllInitialReportsAsync(specParams);
        var response = _mapper.Map<IReadOnlyList<MaintenanceInitialReportResponseDto>>(reports);
        return Ok(response);
    }
    #endregion
}
