using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Vehicles.Category;
using VMTS.API.Errors;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.UnitOfWork;

namespace VMTS.API.Controllers;

[Route("api/Vehicle/Category")]
[ApiController]
public class VehicleCategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<VehicleCategory> _repo;

    public VehicleCategoryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _repo = _unitOfWork.GetRepo<VehicleCategory>();
    }

    [ProducesResponseType<VehicleCategoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<VehicleCategoryDto>> Create(
        VehicleCategoryCreateDto vehilceModel
    )
    {
        var mappedVehicleModel = _mapper.Map<VehicleCategoryCreateDto, VehicleCategory>(
            vehilceModel
        );
        await _repo.CreateAsync(mappedVehicleModel);
        await _unitOfWork.SaveChanges();
        var eturnVehicleModel = await _repo.GetByIdAsync(mappedVehicleModel.Id);
        if (eturnVehicleModel is null)
            return NotFound(new ApiErrorResponse(404));
        var returnVehilceModel = _mapper.Map<VehicleCategory, VehicleCategoryDto>(
            eturnVehicleModel
        );
        return Ok(returnVehilceModel);
    }

    [ProducesResponseType<IReadOnlyList<VehicleCategoryDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VehicleCategoryDto>>> GetAll()
    {
        var allVehicleModels = await _repo.GetAllAsync();
        var returnVehicleList = _mapper.Map<
            IReadOnlyList<VehicleCategory>,
            IReadOnlyList<VehicleCategoryDto>
        >(allVehicleModels);
        return Ok(returnVehicleList);
    }
}
