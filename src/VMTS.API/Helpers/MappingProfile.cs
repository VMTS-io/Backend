using AutoMapper;
using VMTS.API.Dtos;
using VMTS.API.Dtos.Maintenance;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddressDto, Address>().ReverseMap();

        CreateMap<FaultReport, FaultReportResponse>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.ReportedAt));

        CreateMap<MaintenanceRequestDto, MaintenaceRequest>();
        CreateMap<MaintenaceRequest, MaintenanceRequestResponse>();
    }
}
