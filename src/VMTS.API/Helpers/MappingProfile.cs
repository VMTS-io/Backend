using AutoMapper;
using VMTS.API.Dtos;
using VMTS.API.Dtos.Maintenance;
using VMTS.API.Dtos.Maintenance.Category;
using VMTS.API.Dtos.Maintenance.Report;
using VMTS.API.Dtos.Maintenance.Report.Initial;
using VMTS.API.Dtos.Maintenance.Request;
using VMTS.API.Dtos.Part;
using VMTS.API.Dtos.Trip;
using VMTS.API.Dtos.TripReport;
using VMTS.API.Dtos.Vehicles;
using VMTS.API.Dtos.Vehicles.Brand;
using VMTS.API.Dtos.Vehicles.Category;
using VMTS.API.Dtos.Vehicles.Model;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TripReport, TripReportResponse>();

        CreateMap<BusinessUser, BussinessUserDto>();

        CreateMap<AddressDto, Address>().ReverseMap();

        CreateMap<FaultReport, FaultReportResponse>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.ReportedAt));

        CreateMap<TripRequest, TripRequestObj>();

        CreateMap<AddressDto, Address>()
            .ForMember(dest => dest.AppUserId, opt => opt.Ignore()) // Explicitly ignore this
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Apply null-check to all others

        CreateMap<AppUser, UserResponse>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
            .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId));

        CreateMap<MaintenanceRequestDto, MaintenaceRequest>();
        CreateMap<MaintenaceRequest, MaintenanceRequestResponse>();
        CreateMap<VehicleUpsertDto, Vehicle>()
            .ForMember(
                dest => dest.ModelYear,
                opt => opt.MapFrom(src => new DateOnly(src.ModelYear, 1, 1))
            )
            .ForMember(
                dest => dest.JoinedYear,
                opt => opt.MapFrom(src => new DateOnly(src.JoinedYear, 1, 1))
            );

        CreateMap<AppUser, DriverDto>();
        CreateMap<BusinessUser, DriverDto>();
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.VehicleModelDto, opt => opt.MapFrom(src => src.VehicleModel));
        CreateMap<TripRequest, TripDto>();

        CreateMap<Vehicle, AdminVehicleListDto>()
            .ForMember(
                dest => dest.Name,
                opt =>
                    opt.MapFrom(src =>
                        $"{src.VehicleModel.Brand.Name} {src.VehicleModel.Name} {src.ModelYear.Year}"
                    )
            )
            .ForMember(dest => dest.ModelYear, opt => opt.MapFrom(src => src.ModelYear.Year));

        CreateMap<Vehicle, VehicleDetailsDto>()
            .ForMember(dest => dest.ModelYear, opt => opt.MapFrom(src => src.ModelYear.Year));

        CreateMap<Vehicle, VehicleListDto>()
            .ForMember(dest => dest.ModelYear, opt => opt.MapFrom(src => src.ModelYear.Year));
        CreateMap<VehicleDto, VehicleCategoryDto>();
        CreateMap<VehicleCategory, VehicleCategoryDto>();
        CreateMap<VehicleCategoryUpsertDto, VehicleCategory>();
        CreateMap<VehicleModel, VehicleModelDto>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
        ;
        CreateMap<VehicleModelUpsertDto, VehicleModel>();
        CreateMap<TripRequest, TripRequestDto>();
        CreateMap<TripReport, TripReportDto>();
        CreateMap<MaintenaceReport, MaintenanceReportDto>();
        CreateMap<MaintenaceRequest, VehicleMaintenanceRequestDto>();

        CreateMap<MaintenanceInitialReportRequestDto, MaintenanceInitialReport>();

        CreateMap<MaintenanceInitialReport, MaintenanceInitialReportResponseDto>()
            // .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager.DisplayName))
            .ForMember(
                dest => dest.MechanicName,
                opt => opt.MapFrom(src => src.Mechanic.DisplayName)
            )
            .ForMember(
                dest => dest.VehicleName,
                opt => opt.MapFrom(src => src.Vehicle.PalletNumber)
            )
            .ForMember(
                dest => dest.RequestTitle,
                opt => opt.MapFrom(src => src.MaintenanceRequest.Description)
            )
            .ForMember(
                dest => dest.CategoryNames,
                opt => opt.MapFrom(src => src.MaintenanceCategory.Categorty.ToString())
            )
            .ForMember(
                dest => dest.MissingPartNames,
                opt => opt.MapFrom(src => src.MissingParts!.Select(p => p.Name))
            )
            .ForMember(
                dest => dest.ExpectedChangedParts,
                opt => opt.MapFrom(src => src.ExpectedChangedParts.Select(p => p.Part.Name))
            );
        // .ForMember
        //     dest => dest.ExpectedChangedParts,
        //     opt => opt.MapFrom(src => src.ExpectedChangedParts!.Select(p => p.Name))
        // );

        CreateMap<MaintenaceCategoryCreateUpdateDto, MaintenaceCategory>();
        CreateMap<MaintenaceCategory, MaintenaceCategoryResponseDto>();
        CreateMap<Part, PartDto>();
        CreateMap<Brand, BrandDto>();
        CreateMap<CreateOrUpdateBrandDto, Brand>();
        CreateMap<CreateOrUpdatePartDto, Part>();
        CreateMap<MaintenanceReportPartDto, MaintenanceInitialReportParts>().ReverseMap();
        CreateMap<MaintenanceReportPartDto, MaintenanceFinalReportParts>().ReverseMap();
        // .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.VehicleCategory.Name));
    }
}
