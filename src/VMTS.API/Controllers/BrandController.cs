using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Vehicles.Brand;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Controllers;

[Route("api/Vehicle/Brand")]
[Tags("Vehicle/Brand")]
public class BrandController : BaseApiController
{
    private readonly IBrandService _brandService;

    private readonly IMapper _mapper;

    public BrandController(IBrandService brandService, IMapper mapper)
    {
        _brandService = brandService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BrandDto>>> GetAll()
    {
        var brands = await _brandService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<BrandDto>>(brands));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BrandDto>> GetById(string id)
    {
        var brand = await _brandService.GetByIdAsync(id);
        return Ok(_mapper.Map<BrandDto>(brand));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrUpdateBrandDto dto)
    {
        var brand = _mapper.Map<Brand>(dto);
        await _brandService.CreateAsync(brand);
        return CreatedAtAction(
            nameof(GetById),
            new { id = brand.Id },
            _mapper.Map<BrandDto>(brand)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, CreateOrUpdateBrandDto dto)
    {
        var brand = _mapper.Map<Brand>(dto);
        brand.Id = id;
        await _brandService.UpdateAsync(brand);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _brandService.DeleteAsync(id);
        return NoContent();
    }
}
