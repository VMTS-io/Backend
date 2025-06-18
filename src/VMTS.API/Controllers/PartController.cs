using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Part;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Controllers;

public class PartController : BaseApiController
{
    private readonly IPartService _partService;
    private readonly IMapper _mapper;

    public PartController(IPartService partService, IMapper mapper)
    {
        _partService = partService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<PartDto>>> GetAll()
    {
        var parts = await _partService.GetAllAsync();
        return Ok(_mapper.Map<List<PartDto>>(parts));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PartDto>> GetById(string id)
    {
        var part = await _partService.GetByIdAsync(id);
        return Ok(_mapper.Map<PartDto>(part));
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateOrUpdatePartDto dto)
    {
        var part = _mapper.Map<Part>(dto);
        await _partService.CreateAsync(part);
        return CreatedAtAction(nameof(GetById), new { id = part.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, CreateOrUpdatePartDto dto)
    {
        var part = _mapper.Map<Part>(dto);
        part.Id = id;
        await _partService.UpdateAsync(part);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await _partService.DeleteAsync(id);
        return NoContent();
    }
}
