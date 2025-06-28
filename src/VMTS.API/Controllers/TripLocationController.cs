using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Controllers;

public class TripLocationController : BaseApiController
{
    private readonly ITripLocationService _locationService;
    private readonly IMapper _mapper;

    public TripLocationController(ITripLocationService locationService, IMapper mapper)
    {
        _locationService = locationService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TripLocationDto dto)
    {
        var mappedmodel = _mapper.Map<TripLocation>(dto);
        await _locationService.SetLocationAsync(mappedmodel);
        var status = HttpContext.Response.StatusCode;
        return Ok(new { StatusCode = status });
    }

    [HttpGet("{tripId}")]
    public async Task<ActionResult<TripLocationDto>> Get(string tripId)
    {
        var result = await _locationService.GetLocationAsync(tripId);
        if (result is null)
            return NotFound();
        var status = HttpContext.Response.StatusCode;
        return Ok(new { Data = result, StatusCode = status });
    }
}
