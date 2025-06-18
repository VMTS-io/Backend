using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.ActionFilters;
using VMTS.API.Dtos.Vehicles.Category;
using VMTS.API.Errors;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Controllers;

[Tags("Vehicle/Category")]
[Route("api/Vehicle/Category")]
public class VehicleCategoryController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IValidator<VehicleCategoryUpsertDto> _validator;
    private readonly IVehicleCategoryServices _services;

    public VehicleCategoryController(
        IMapper mapper,
        IValidator<VehicleCategoryUpsertDto> validator,
        IVehicleCategoryServices services
    )
    {
        _mapper = mapper;
        _validator = validator;
        _services = services;
    }

    #region Get All
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<VehicleCategoryDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<VehicleCategoryDto>>> GetAll()
    {
        var vehicleCategoryList = await _services.GetAllVehicleCategoryAsync();
        var vehicleCategoryDtoList = _mapper.Map<
            IReadOnlyList<VehicleCategory>,
            IReadOnlyList<VehicleCategoryDto>
        >(vehicleCategoryList);
        return Ok(vehicleCategoryDtoList);
    }
    #endregion

    #region Create
    [HttpPost]
    [ServiceFilter<ValidateModelActionFilter<VehicleCategoryUpsertDto>>]
    [ProducesResponseType<VehicleCategoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleCategoryDto>> Create(VehicleCategoryUpsertDto createDto)
    {
        // var validationResult = _validator.Validate(createDto);
        // if (!validationResult.IsValid)
        //     return BadRequest(ValidationProblemDetails(validationResult.ToDictionary()));

        var vehicleCategory = _mapper.Map<VehicleCategoryUpsertDto, VehicleCategory>(createDto);
        vehicleCategory = await _services.CreateVehicleCategoryAsync(vehicleCategory);

        var categoryDto = _mapper.Map<VehicleCategory, VehicleCategoryDto>(vehicleCategory);
        return Ok(categoryDto);
    }
    #endregion

    #region Update
    [HttpPut("{id}")]
    [ServiceFilter<ValidateModelActionFilter<VehicleCategoryUpsertDto>>]
    [ProducesResponseType<VehicleCategoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleCategoryDto>> Update(
        [FromRoute] string id,
        [FromBody] VehicleCategoryUpsertDto updateDto
    )
    {
        // var validationResult = _validator.Validate(updateDto);
        // if (!validationResult.IsValid)
        //     return BadRequest(ValidationProblemDetails(validationResult.ToDictionary()));

        var vehicleCategory = _mapper.Map<VehicleCategoryUpsertDto, VehicleCategory>(updateDto);
        vehicleCategory.Id = id;
        vehicleCategory = await _services.UpdateVehicleCategory(vehicleCategory);
        var vehicleCategoryDto = _mapper.Map<VehicleCategory, VehicleCategoryDto>(vehicleCategory!);
        return Ok(vehicleCategoryDto);
    }
    #endregion

    #region Delete
    [ProducesResponseType<bool>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await _services.DeleteVehicleCategory(id);
        return Ok();
    }
    #endregion
}
