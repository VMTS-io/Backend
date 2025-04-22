using AutoMapper;
using VMTS.API.Dtos;
using VMTS.API.Dtos.Maintenance;
using VMTS.API.Dtos.Trip;
using VMTS.API.Dtos.Vehicles;
using VMTS.API.Dtos.Vehicles.Category;
using VMTS.API.Dtos.Vehicles.Model;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddressDto, Address>().ReverseMap();

        CreateMap<FaultReport, FaultReportResponse>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.ReportedAt));

        CreateMap<AddressDto, Address>()
            .ForMember(dest => dest.AppUserId, opt => opt.Ignore()) // Explicitly ignore this
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Apply null-check to all others

        CreateMap<AppUser, UserResponse>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
            .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId));

        CreateMap<MaintenanceRequestDto, MaintenaceRequest>();
        CreateMap<MaintenaceRequest, MaintenanceRequestResponse>();
        CreateMap<Vehicle, VehicleCreateRequest>().ReverseMap();
        CreateMap<Vehicle, VehicleDetailsDto>();
        CreateMap<Vehicle, VehicleListDto>();
        CreateMap<VehicleUpdateRequest, Vehicle>();
        CreateMap<VehicleCategory, VehicleCategoryDto>();
        CreateMap<VehicleCategoryCreateDto, VehicleCategory>();
        CreateMap<VehicleModel, VehicleModelDto>();
        CreateMap<VehicleModelCreateDto, VehicleModel>();
        CreateMap<TripRequest, TripRequestDto>();
        CreateMap<TripReport, TripReportDto>();
        CreateMap<MaintenaceReport, MaintenanceReportDto>();
        CreateMap<MaintenaceRequest, VehicleMaintenanceRequestDto>();
        CreateMap<Vehicle, AdminVehicleListDto>()
            .ForMember(
                dest => dest.Name,
                opt =>
                    opt.MapFrom(src =>
                        $"{src.VehicleModel.Manufacturer} {src.VehicleModel.Year.Year}"
                    )
            )
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.VehicleCategory.Name));
    }
}
