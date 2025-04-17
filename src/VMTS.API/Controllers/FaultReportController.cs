using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos;
using VMTS.API.Errors;
using VMTS.Core.Entities.Identity;
using VMTS.Core.ServicesContract;
using VMTS.Service.Services;

namespace VMTS.API.Controllers;


public class FaultReportController : BaseApiController
{
    private readonly IReportService _ireportService;
    private readonly IMapper _mapper;


    public FaultReportController(
        IReportService ireportService,
        IMapper mapper)
    {
        _ireportService = ireportService;
        _mapper = mapper;
    }
   
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Driver")]
    [HttpPost("create")]
    public async Task<ActionResult<FaultReportResponse>> CreateFaultReportAsync(FaultReportRequest request)
    {
        var user = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(user))
            return Unauthorized(new ApiResponse(401));

        var faultReport = await _ireportService.CreateFaultReportAsync(
            user,
            request.Details,
            request.FaultType,
            request.Address
        );
    
        if (faultReport is null)
            return BadRequest(new ApiResponse(400, "Failed to create fault report"));

        var mappedReport = _mapper.Map<FaultReportResponse>(faultReport);
        return Ok(mappedReport);
    }


}
