using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Maintenance.Category;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Controllers;

[ApiController]
[Route("api/Maintenance/Category")]
[Tags("Maintenance/Category")]
public class MaintenaceCategoryController : ControllerBase
{
    private readonly IMaintenanceCategoryServices _categoryService;
    private readonly IMapper _mapper;

    public MaintenaceCategoryController(
        IMaintenanceCategoryServices categoryService,
        IMapper mapper
    )
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    #region Get All
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaintenaceCategoryResponseDto>>> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        var dto = _mapper.Map<IEnumerable<MaintenaceCategoryResponseDto>>(categories);
        return Ok(dto);
    }
    #endregion

    #region Get By Id
    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenaceCategoryResponseDto>> GetById(string id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        return Ok(_mapper.Map<MaintenaceCategoryResponseDto>(category));
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MaintenaceCategoryCreateUpdateDto dto)
    {
        var category = _mapper.Map<MaintenaceCategories>(dto);
        await _categoryService.CreateCategoryAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, null);
    }
    #endregion

    #region Update eiJEif
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] string id,
        [FromBody] MaintenaceCategoryCreateUpdateDto dto
    )
    {
        var category = _mapper.Map<MaintenaceCategories>(dto);
        category.Id = id;
        await _categoryService.UpdateCategoryAsync(category);
        return NoContent();
    }
    #endregion

    #region Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }
    #endregion
}
