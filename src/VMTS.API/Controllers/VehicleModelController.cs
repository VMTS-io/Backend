using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.ActionFilters;
using VMTS.API.Dtos.Vehicles.Model;
using VMTS.API.Errors;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Controllers;

[Tags("Vehicle/Models")]
[Route("api/Vehicle/Model")]
public class VehicleModelController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IValidator<VehicleModelUpsertDto> _validator;
    private readonly IVehicleModelServices _services;

    public VehicleModelController(
        IMapper mapper,
        IValidator<VehicleModelUpsertDto> validator,
        IVehicleModelServices services
    )
    {
        _mapper = mapper;
        _validator = validator;
        _services = services;
    }

    #region Create
    [HttpPost]
    [ServiceFilter<ValidateModelActionFilter<VehicleModelUpsertDto>>]
    [ProducesResponseType<VehicleModelDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleModelDto>> Create(VehicleModelUpsertDto createDto)
    {
        // var validationResult = _validator.Validate(createDto);
        // if (!validationResult.IsValid)
        //     return BadRequest(ValidationProblemDetails(validationResult.ToDictionary()));

        var vehicleModel = _mapper.Map<VehicleModelUpsertDto, VehicleModel>(createDto);
        await _services.CreateVehicleModelAsync(vehicleModel);
        return NoContent();
    }
    #endregion

    #region Update
    [HttpPut("{id}")]
    [ServiceFilter<ValidateModelActionFilter<VehicleModelUpsertDto>>]
    [ProducesResponseType<VehicleModelDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(
        [FromRoute] string id,
        [FromBody] VehicleModelUpsertDto UpdateDto
    )
    {
        // var validationResult = _validator.Validate(UpdateDto);
        // if (!validationResult.IsValid)
        //     return BadRequest(ValidationProblemDetails(validationResult.ToDictionary()));

        var vehicleModel = _mapper.Map<VehicleModelUpsertDto, VehicleModel>(UpdateDto);
        vehicleModel.Id = id;
        await _services.UpdateVehicleModelAsync(vehicleModel);
        return NoContent();
    }
    #endregion

    #region Delete
    [HttpDelete("{id}")]
    [ProducesResponseType<bool>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(string id)
    {
        await _services.DeleteVehicleModelAsync(id);
        return Ok();
    }
    #endregion

    #region GetAll
    [ProducesResponseType<IReadOnlyList<VehicleModelDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VehicleModelDto>>> GetAll(
        [FromQuery] string? categroyId
    )
    {
        var vehicleModelsList = await _services.GetAllVehicleModelsAsync(categroyId);
        var VehicleModelDtoList = _mapper.Map<
            IReadOnlyList<VehicleModel>,
            IReadOnlyList<VehicleModelDto>
        >(vehicleModelsList);
        return Ok(VehicleModelDtoList);
    }
    #endregion
}
