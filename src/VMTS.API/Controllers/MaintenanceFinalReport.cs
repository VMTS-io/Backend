using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Maintenance.Report.Final;
using VMTS.Core.Entities.Maintenace;
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
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MaintenanceFinalReportRequestDto dto)
    {
        var mechanicId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var report = _mapper.Map<MaintenanceFinalReport>(dto);
        report.MechanicId = mechanicId!;
        await _service.CreateFinalReportAsync(report, dto.CategoryIds, dto.PartIds);
        return Ok();
    }
    #endregion

    #region Update
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] string id,
        [FromBody] MaintenanceFinalReportRequestDto dto
    )
    {
        var report = _mapper.Map<MaintenanceFinalReport>(dto);
        report.Id = id;
        await _service.UpdateFinalReportAsync(report, dto.CategoryIds, dto.PartIds);
        return NoContent();
    }
    #endregion

    #region Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _service.DeleteFinalReportAsync(id);
        return NoContent();
    }
    #endregion

    #region Get By Id
    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceFinalReportResponseDto>> GetById(
        [FromRoute] string id
    )
    {
        var report = await _service.GetFinalReportByIdAsync(id);
        var response = _mapper.Map<MaintenanceFinalReportResponseDto>(report);
        return Ok(response);
    }
    #endregion

    #region GetAll
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaintenanceFinalReportResponseDto>>> GetAll(
        [FromQuery] MaintenanceFinalReportSpecParams specParams
    )
    {
        var reports = await _service.GetAllFinalReportsAsync(specParams);
        var response = _mapper.Map<IReadOnlyList<MaintenanceFinalReportResponseDto>>(reports);
        return Ok(response);
    }
    #endregion
}
