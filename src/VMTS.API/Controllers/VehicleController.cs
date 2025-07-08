using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.ActionFilters;
using VMTS.API.Dtos.Vehicles;
using VMTS.API.Errors;
using VMTS.API.Helpers;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Specifications.VehicleSpecification;

namespace VMTS.API.Controllers;

public class VehicleController : BaseApiController
{
    private readonly IVehicleSerivces _services;
    private readonly IPartService _partServices;
    private readonly IMapper _mapper;

    public VehicleController(IVehicleSerivces services, IPartService partServices, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
        _partServices = partServices;
    }

    #region Get All
    [ProducesResponseType<IReadOnlyList<VehicleListDto>>(StatusCodes.Status200OK)]
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
    [HttpPost]
    [ServiceFilter<ValidateModelActionFilter<VehicleUpsertDto>>]
    [ProducesResponseType<VehicleDetailsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleDetailsDto>> Create([FromBody] VehicleUpsertDto vehicle)
    {
        // var validationResult = _validator.Validate(vehicle);
        // if (!validationResult.IsValid)
        //     return BadRequest(ValidationProblemDetails(validationResult.ToDictionary()));
        //
        var mappedVehicle = _mapper.Map<VehicleUpsertDto, Vehicle>(vehicle);
        var tempVehicle = await _services.CreateVehicleAsync(mappedVehicle);
        var returnVehicle = _mapper.Map<Vehicle, VehicleListDto>(tempVehicle);
        return Ok(returnVehicle);
    }
    #endregion

    #region Update
    [HttpPut("{id}")]
    [ServiceFilter<ValidateModelActionFilter<VehicleUpsertDto>>]
    [ProducesResponseType<VehicleDetailsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleDetailsDto>> Update(
        [FromRoute] string id,
        [FromBody] VehicleUpsertDto vehicle
    )
    {
        // var validationResult = _validator.Validate(vehicle);
        // if (!validationResult.IsValid)
        //     return BadRequest(new HttpValidationProblemDetails(validationResult.ToDictionary()));
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

    #region Download template
    [HttpGet("download-template")]
    public async Task<IActionResult> DownloadVehicleTemplate()
    {
        var bytes = await ExcelFile.GenerateVehicleTemplate(_partServices);

        return File(
            bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "VehicleTemplate.xlsx"
        );
    }

    #endregion

    #region Create with history with json
    [HttpPost("with-list-history")]
    [ServiceFilter<ValidateModelActionFilter<VehicleUpsertDto>>]
    [ProducesResponseType<VehicleDetailsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleDetailsDto>> Create(
        [FromBody] VehicleWithHistoryDto vehicleWithHistory
    )
    {
        var mappedVehicle = _mapper.Map<Vehicle>(vehicleWithHistory.Vehicle);
        var mappedHistory = _mapper.Map<List<MaintenanceTracking>>(
            vehicleWithHistory.MaintenanceHistory,
            opts => opts.Items["VehicleId"] = mappedVehicle.Id
        );

        await _services.CreateVehicleWithHistoryAsync(mappedVehicle, mappedHistory);

        return NoContent();
    }
    #endregion

    #region Create With History with excel
    [HttpPost("with-excel-history")]
    [Consumes("multipart/form-data")]
    [ServiceFilter<ValidateModelActionFilter<VehicleUpsertDto>>]
    [ProducesResponseType<VehicleDetailsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CreateWithHistory(
        [FromForm] VehicleUpsertDto vehicle,
        [FromForm] IFormFile file
    )
    {
        var mappedVehicle = _mapper.Map<VehicleUpsertDto, Vehicle>(vehicle);
        var (list, errors) = await ExcelFile.ParseExcelAsync(file, _partServices, mappedVehicle.Id);

        if (errors.Count != 0)
            return BadRequest(new { Message = "Validation failed.", Errors = errors });

        await _services.CreateVehicleWithHistoryAsync(mappedVehicle, list);
        return NoContent();
    }
    #endregion
}
