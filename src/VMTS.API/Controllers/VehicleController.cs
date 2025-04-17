using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Vehicles;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Specifications.VehicleSpecification;

namespace VMTS.API.Controllers;

public class VehicleController : BaseApiController
{
    private readonly IVehicleSerivces _services;
    private readonly IMapper _mapper;

    public VehicleController(IVehicleSerivces services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VehicleListDto>>> GetAll(
        [FromQuery] VehicleSpecParams specParams
    )
    {
        var vehicles = await _services.GetAllVehiclesAsync(specParams);
        var returnVehicle = _mapper.Map<IReadOnlyList<Vehicle>, IReadOnlyList<VehicleListDto>>(
            vehicles
        );
        return Ok(returnVehicle);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDetailsDto>> GetById([FromRoute] string id)
    {
        var vehicle = await _services.GetVehicleByIdAsync(id);
        var mappedVehicle = _mapper.Map<Vehicle, VehicleDetailsDto>(vehicle);
        return Ok(mappedVehicle);
    }

    [HttpPost]
    public async Task<ActionResult<VehicleDetailsDto>> Create(VehicleCreateRequest vehicle)
    {
        var mappedVehicle = _mapper.Map<VehicleCreateRequest, Vehicle>(vehicle);
        var tempVehicle = await _services.CreateVehicleAsync(mappedVehicle);
        var returnVehicle = _mapper.Map<Vehicle, VehicleListDto>(tempVehicle);
        return Ok(returnVehicle);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete([FromRoute] string id)
    {
        return Ok(await _services.DeleteVehicleAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VehicleDetailsDto>> Update(
        [FromRoute] string id,
        [FromBody] VehicleUpdateRequest vehicle
    )
    {
        var mappedVehicle = _mapper.Map<VehicleUpdateRequest, Vehicle>(vehicle);
        mappedVehicle.Id = id;
        var updatedVehicle = await _services.UpdateVehicleAsync(mappedVehicle);
        var returnVehicle = _mapper.Map<Vehicle, VehicleDetailsDto>(updatedVehicle);
        return Ok(returnVehicle);
    }
}
