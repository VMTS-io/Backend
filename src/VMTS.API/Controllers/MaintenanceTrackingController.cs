using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Maintenance.Tracking;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Controllers;

[Route("api/Maintenance/Tracking")]
[Tags("Maintenance/Tracking")]
[ApiController]
public class MaintenanceTrackingController : BaseApiController
{
    private readonly IMaintenanceTrackingServices _service;
    private readonly IMapper _mapper;

    public MaintenanceTrackingController(IMaintenanceTrackingServices service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> Create(MaintenanceTrackingCreateDto parameter)
    {
        var mappedEntity = _mapper.Map<MaintenanceTracking>(parameter);
        await _service.Create(mappedEntity);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(
        [FromBody] MaintenanceTrackingCreateDto parameter,
        [FromRoute] string id
    )
    {
        var mappedEntity = _mapper.Map<MaintenanceTracking>(parameter);
        mappedEntity.Id = id;
        await _service.UpdateAll(mappedEntity);
        return NoContent();
    }
}
