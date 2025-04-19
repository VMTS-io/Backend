using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Vehicles;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.UnitOfWork;

namespace VMTS.API.Controllers;

[Route("api/vehicle/category")]
[ApiController]
public class VehicleCategoryController: ControllerBase
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

    [HttpPost]
    public async Task<ActionResult<VehicleCategoryDto>> Create(VehicleCategoryDto vehilceModel)
    {
        var mappedVehicleModel = _mapper.Map<VehicleCategoryDto, VehicleCategory>(vehilceModel);
        await _repo.CreateAsync(mappedVehicleModel);
        var eturnVehicleModel = await _repo.GetByIdAsync(mappedVehicleModel.Id);
        var returnVehilceModel = _mapper.Map<VehicleCategory, VehicleCategoryDto>(
            eturnVehicleModel
        );
        return Ok(returnVehilceModel);
    }

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
