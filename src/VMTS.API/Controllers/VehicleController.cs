using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Vehicles;
using VMTS.API.Errors;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Specifications.VehicleSpecification;

namespace VMTS.API.Controllers;

public class VehicleController : BaseApiController
{
    private readonly IVehicleSerivces _services;
    private readonly IMapper _mapper;
    private readonly IValidator<VehicleUpsertDto> _validator;

    public VehicleController(
        IVehicleSerivces services,
        IMapper mapper,
        IValidator<VehicleUpsertDto> validator
    )
    {
        _services = services;
        _mapper = mapper;
        _validator = validator;
    }

    // [ProducesResponseType<IReadOnlyList<VehicleListDto>>(StatusCodes.Status200OK)]
    // [HttpGet]
    // public async Task<ActionResult<IReadOnlyList<VehicleListDto>>> GetAll(
    //     [FromQuery] VehicleSpecParams specParams
    // )
    // {
    //     var vehicles = await _services.GetAllVehiclesAsync(specParams);
    //     var returnVehicle = _mapper.Map<IReadOnlyList<Vehicle>, IReadOnlyList<VehicleListDto>>(
    //         vehicles
    //     );
    //     return Ok(returnVehicle);
    // }


    #region Get All
    [ProducesResponseType<IReadOnlyList<AdminVehicleListDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AdminVehicleListDto>>> GetAll(
        [FromQuery] VehicleSpecParams specParams
    )
    {
        var vehicles = await _services.GetAllVehiclesAsync(specParams);
        var returnVehicle = _mapper.Map<IReadOnlyList<Vehicle>, IReadOnlyList<AdminVehicleListDto>>(
            vehicles
        );
        return Ok(returnVehicle);
    }
    #endregion

    #region Get By Id
    [ProducesResponseType<VehicleDetailsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDetailsDto>> GetById([FromRoute] string id)
    {
        var vehicle = await _services.GetVehicleByIdAsync(id);
        var mappedVehicle = _mapper.Map<Vehicle, VehicleDetailsDto>(vehicle);
        return Ok(mappedVehicle);
    }
    #endregion

    #region Create
    [ProducesResponseType<VehicleDetailsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<VehicleDetailsDto>> Create(VehicleUpsertDto vehicle)
    {
        var validationResult = _validator.Validate(vehicle);
        if (!validationResult.IsValid)
            return BadRequest(ValidationProblemDetails(validationResult.ToDictionary()));

        var mappedVehicle = _mapper.Map<VehicleUpsertDto, Vehicle>(vehicle);
        var tempVehicle = await _services.CreateVehicleAsync(mappedVehicle);
        var returnVehicle = _mapper.Map<Vehicle, VehicleListDto>(tempVehicle);
        return Ok(returnVehicle);
    }
    #endregion

    #region Update
    [ProducesResponseType<VehicleDetailsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<ActionResult<VehicleDetailsDto>> Update(
        [FromRoute] string id,
        [FromBody] VehicleUpsertDto vehicle
    )
    {
        var validationResult = _validator.Validate(vehicle);
        if (!validationResult.IsValid)
            return BadRequest(new HttpValidationProblemDetails(validationResult.ToDictionary()));

        var mappedVehicle = _mapper.Map<VehicleUpsertDto, Vehicle>(vehicle);
        mappedVehicle.Id = id;
        var updatedVehicle = await _services.UpdateVehicleAsync(mappedVehicle);
        var returnVehicle = _mapper.Map<Vehicle, VehicleDetailsDto>(updatedVehicle);
        return Ok(returnVehicle);
    }
    #endregion

    #region Delete
    [ProducesResponseType<bool>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete([FromRoute] string id)
    {
        return Ok(await _services.DeleteVehicleAsync(id));
    }
    #endregion
}
