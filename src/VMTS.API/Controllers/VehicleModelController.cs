using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Vehicles;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.UnitOfWork;

namespace VMTS.API.Controllers;

[Route("api/vehicle/model")]
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

    [HttpPost]
    public async Task<ActionResult<VehicleModelDto>> Create(VehicleModelDto vehilceModel)
    {
        var mappedVehicleModel = _mapper.Map<VehicleModelDto, VehicleModel>(vehilceModel);
        await _repo.CreateAsync(mappedVehicleModel);
        var eturnVehicleModel = await _repo.GetByIdAsync(mappedVehicleModel.Id);
        var returnVehilceModel = _mapper.Map<VehicleModel, VehicleModelDto>(eturnVehicleModel);
        return Ok(returnVehilceModel);
    }

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
}
