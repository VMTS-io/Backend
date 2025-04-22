using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Vehicles.Model;
using VMTS.API.Errors;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.UnitOfWork;

namespace VMTS.API.Controllers;

[Tags("Vehicle/Models")]
[Route("api/Vehicle/model")]
[ApiController]
public class VehicleModelController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<VehicleModel> _repo;

    public VehicleModelController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _repo = _unitOfWork.GetRepo<VehicleModel>();
    }

    [ProducesResponseType<VehicleModelDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<VehicleModelDto>> Create(VehicleModelCreateDto vehilceModel)
    {
        var mappedVehicleModel = _mapper.Map<VehicleModelCreateDto, VehicleModel>(vehilceModel);
        await _repo.CreateAsync(mappedVehicleModel);
        await _unitOfWork.SaveChanges();
        var eturnVehicleModel = await _repo.GetByIdAsync(mappedVehicleModel.Id);
        if (eturnVehicleModel is null)
            return NotFound(new ApiErrorResponse(404));
        var returnVehilceModel = _mapper.Map<VehicleModel, VehicleModelDto>(eturnVehicleModel);
        return Ok(returnVehilceModel);
    }

    [ProducesResponseType<IReadOnlyList<VehicleModelDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VehicleModelDto>>> GetAll()
    {
        var allVehicleModels = await _repo.GetAllAsync();
        var returnVehicleList = _mapper.Map<
            IReadOnlyList<VehicleModel>,
            IReadOnlyList<VehicleModelDto>
        >(allVehicleModels);
        return Ok(returnVehicleList);
    }

    [ProducesResponseType<VehicleModelDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<ActionResult<VehicleModelDto>> Edit(
        [FromRoute] string id,
        [FromBody] VehicleModelCreateDto model
    )
    {
        var mappedModel = _mapper.Map<VehicleModelCreateDto, VehicleModel>(model);
        var vehicleModel = await _repo.GetByIdAsync(id);
        if (vehicleModel is null)
            return BadRequest(
                new ApiErrorResponse(
                    StatusCodes.Status404NotFound,
                    $"Veicle Model Not Found With Id {id}"
                )
            );
        mappedModel.Id = id;
        _repo.Update(mappedModel);
        await _unitOfWork.SaveChanges();
        vehicleModel = await _repo.GetByIdAsync(id);
        var returnVehicleModel = _mapper.Map<VehicleModel, VehicleModelDto>(vehicleModel!);
        return Ok(returnVehicleModel!);
    }

    [ProducesResponseType<bool>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(string id)
    {
        var vehicleModel = await _repo.GetByIdAsync(id);
        if (vehicleModel is null)
            return BadRequest(
                new ApiErrorResponse(
                    StatusCodes.Status404NotFound,
                    $"Veicle Model Not Found With Id {id}"
                )
            );
        _repo.Delete(vehicleModel);
        await _unitOfWork.SaveChanges();
        return true;
    }
}
