using AutoMapper;
using VMTS.API.Dtos;
using VMTS.API.Dtos.DriverReportsResponse;
using VMTS.API.Dtos.Maintenance.Category;
using VMTS.API.Dtos.Maintenance.Report;
using VMTS.API.Dtos.Maintenance.Report.Final;
using VMTS.API.Dtos.Maintenance.Report.Initial;
using VMTS.API.Dtos.Maintenance.Request;
using VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;
using VMTS.API.Dtos.Part;
using VMTS.API.Dtos.Trip;
using VMTS.API.Dtos.TripReport;
using VMTS.API.Dtos.Vehicles;
using VMTS.API.Dtos.Vehicles.Category;
using VMTS.API.Dtos.Vehicles.Model;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Non_Entities_Class;

namespace VMTS.API.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FaultReport, DriverReportItemDto>()
            .ForMember(dest => dest.FuelCost, opt => opt.MapFrom(src => src.Cost))
            .ForMember(dest => dest.ReportType, opt => opt.MapFrom(_ => "Fault")); // if needed

        CreateMap<TripRequest, TripRequestForVehicles>()
            .ForMember(tr => tr.DriverName, opt => opt.MapFrom(src => src.Driver.DisplayName));

        CreateMap<TripLocationDto, TripLocation>().ReverseMap();
        CreateMap<BusinessUser, BusinessUserGetAllResponse>();

        CreateMap<VehicleWithDueParts, VehicleWithDuePartsDto>();
        CreateMap<VehicleWithDueParts, VehicleTrackingDto>();
        CreateMap<DuePart, DuePartDto>();
        CreateMap<TripReport, DriverReportItemDto>()
            .ForMember(dest => dest.ReportType, opt => opt.MapFrom(_ => "Trip"))
            .ForMember(dest => dest.Driver, opt => opt.MapFrom(src => src.Driver))
            .ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => src.Vehicle))
            .ForMember(dest => dest.ReportedAt, opt => opt.MapFrom(src => src.ReportedAt))
            .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Destination))
            .ForMember(dest => dest.FuelCost, opt => opt.MapFrom(src => src.FuelCost));

        // Map FaultReport -> DriverReportItemDto
        CreateMap<FaultReport, DriverReportItemDto>()
            .ForMember(dest => dest.ReportType, opt => opt.MapFrom(_ => "Fault"))
            .ForMember(dest => dest.Driver, opt => opt.MapFrom(src => src.Driver))
            .ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => src.Vehicle))
            .ForMember(dest => dest.ReportedAt, opt => opt.MapFrom(src => src.ReportedAt))
            .ForMember(dest => dest.FaultDetails, opt => opt.MapFrom(src => src.Details))
            .ForMember(dest => dest.FaultType, opt => opt.MapFrom(src => src.AiPredictedFaultType))
            .ForMember(dest => dest.FuelCost, opt => opt.MapFrom(src => src.Cost));

        // Map nested objects

        // CreateMap<Vehicle, VehicleDto>()
        //     .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.VehicleModel.Name))
        //     .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.VehicleModel.Category.Name))
        //     .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.VehicleModel.Brand.Name));

        CreateMap<TripReport, TripReportResponse>();

        CreateMap<BusinessUser, BussinessUserDto>();

        CreateMap<AddressDto, Address>().ReverseMap();

        CreateMap<FaultReport, FaultReportResponse>()
            .ForMember(dest => dest.ReportedAt, opt => opt.MapFrom(src => src.ReportedAt));

        CreateMap<TripRequest, TripRequestResponse>();

        CreateMap<AddressDto, Address>()
            .ForMember(dest => dest.AppUserId, opt => opt.Ignore()) // Explicitly ignore this
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Apply null-check to all others

        CreateMap<AppUser, UserResponse>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
            .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId));

        CreateMap<MaintenanceRequestUpsertDto, MaintenaceRequest>()
            .ForMember(dest => dest.Parts, opts => opts.Ignore());
        CreateMap<MaintenaceRequest, MaintenanceRequestResponseDto>();

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

        CreateMap<Vehicle, VehicleListDto>()
            .ForMember(
                dest => dest.Name,
                opt =>
                    opt.MapFrom(src =>
                        $"{src.VehicleModel.Brand} {src.VehicleModel.Name} {src.ModelYear.Year}"
                    )
            )
            .ForMember(dest => dest.ModelYear, opt => opt.MapFrom(src => src.ModelYear.Year))
            .ForMember(
                dest => dest.Category,
                opt => opt.MapFrom(src => src.VehicleModel.Category.Name)
            );

        CreateMap<Vehicle, VehicleDetailsDto>()
            .ForMember(dest => dest.ModelYear, opt => opt.MapFrom(src => src.ModelYear.Year));

        CreateMap<Vehicle, VehicleListDetailsDto>()
            .ForMember(dest => dest.ModelYear, opt => opt.MapFrom(src => src.ModelYear.Year));
        CreateMap<VehicleDto, VehicleCategoryDto>();
        CreateMap<VehicleCategory, VehicleCategoryDto>();
        CreateMap<VehicleCategoryUpsertDto, VehicleCategory>();
        CreateMap<VehicleModel, VehicleModelDto>();
        CreateMap<VehicleModel, VehicelModelSummary>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<VehicleModelUpsertDto, VehicleModel>();
        CreateMap<TripRequest, TripRequestDto>();
        CreateMap<TripReport, TripReportDto>();
        // CreateMap<MaintenaceReport, MaintenanceReportDto>();
        CreateMap<MaintenaceRequest, VehicleMaintenanceRequestDto>()
            .ForMember(mr => mr.MechanicName, opt => opt.MapFrom(src => src.Mechanic.DisplayName))
            .ForMember(
                mr => mr.ChangedParts,
                opt =>
                    opt.MapFrom(src =>
                        src.FinalReport.ChangedParts.Select(cp => cp.Part)
                            .Concat(src.Parts)
                            .Distinct()
                    )
            );

        CreateMap<MaintenanceInitialReportRequestDto, MaintenanceInitialReport>();

        CreateMap<MaintenanceInitialReportUpdateDto, MaintenanceInitialReport>();
        CreateMap<MaintenanceFinalReportUpdateDto, MaintenanceFinalReport>();
        CreateMap<MaintenanceInitialReport, MaintenanceInitialReportResponseDto>()
            .ForMember(
                dest => dest.MechanicName,
                opt => opt.MapFrom(src => src.Mechanic.DisplayName)
            )
            .ForMember(
                dest => dest.VehicleName,
                opt =>
                    opt.MapFrom(src =>
                        $"{src.Vehicle.VehicleModel.Brand} {src.Vehicle.VehicleModel.Name} {src.Vehicle.PalletNumber}"
                    )
            )
            .ForMember(
                dest => dest.RequestTitle,
                opt => opt.MapFrom(src => src.MaintenanceRequest.Description)
            )
            .ForMember(
                dest => dest.MissingParts,
                opt => opt.MapFrom(src => src.MissingParts!.Select(p => p.Name))
            )
            .ForMember(
                dest => dest.ExpectedChangedParts,
                opt => opt.MapFrom(src => src.ExpectedChangedParts.Select(p => p.Part.Name))
            )
            .ForMember(
                dest => dest.RequestStatus,
                opt => opt.MapFrom(src => src.MaintenanceRequest.Status.ToString())
            );
        CreateMap<MaintenanceFinalReportRequestDto, MaintenanceFinalReport>();
        CreateMap<MaintenanceFinalReport, MaintenanceFinalReportResponseDto>()
            .ForMember(
                dest => dest.MechanicName,
                opt => opt.MapFrom(src => src.Mechanic.DisplayName)
            )
            .ForMember(
                dest => dest.VehicleName,
                opt =>
                    opt.MapFrom(src =>
                        $"{src.Vehicle.VehicleModel.Brand} {src.Vehicle.VehicleModel.Name} {src.Vehicle.PalletNumber}"
                    )
            )
            .ForMember(
                dest => dest.RequestTitle,
                opt => opt.MapFrom(src => src.MaintenaceRequest.Description)
            )
            .ForMember(
                dest => dest.InitialReportSummary,
                opt => opt.MapFrom(src => src.InitialReport.Notes)
            )
            .ForMember(
                dest => dest.ChangedParts,
                opt => opt.MapFrom(src => src.ChangedParts.Select(p => p.Part.Name))
            )
            .ForMember(
                dest => dest.RequestStatus,
                opt => opt.MapFrom(src => src.MaintenaceRequest.Status.ToString())
            );
        // .ForMember
        //     dest => dest.ExpectedChangedParts,
        //     opt => opt.MapFrom(src => src.ExpectedChangedParts!.Select(p => p.Name))
        // );

        CreateMap<MaintenaceCategoryCreateUpdateDto, MaintenaceCategories>();
        CreateMap<MaintenaceCategories, MaintenaceCategoryResponseDto>();
        CreateMap<Part, PartDto>();
        CreateMap<CreateOrUpdatePartDto, Part>();
        CreateMap<MaintenanceReportPartDto, MaintenanceInitialReportParts>().ReverseMap();
        CreateMap<MaintenanceReportPartDto, MaintenanceFinalReportParts>().ReverseMap();
        CreateMap<Part, PartForMaintenanceReportDto>();
        CreateMap<VehicleModel, VehicleModelForMaintenanceReportDto>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<Vehicle, VehicleForMaintenanceReportDto>();
        CreateMap<MaintenaceRequest, MaintenanceRequestForReportDto>();
        CreateMap<MaintenanceInitialReportParts, MaintenanceReportPartResponseDto>();
        CreateMap<MaintenanceFinalReportParts, MaintenanceReportPartResponseDto>();
        CreateMap<MaintenanceInitialReport, MaintenanceInitialReportDetailsDto>();
        CreateMap<MaintenanceFinalReport, MaintenanceFinalReportDetailsDto>();
        CreateMap<MaintenanceInitialReport, MaintenanceInitialReportSummaryDto>();
        CreateMap<MaintenanceFinalReport, MaintenanceFinalReportSummaryDto>();
        CreateMap<MaintenaceRequest, MaintenanceRequestResponseDto>();

        // .ForMember(
        //     dest => dest.MaintenaceCategory,
        //     opts => opts.MapFrom(src => $"{src.MaintenanceCategory.Categorty} Maintenance")
        // );
    }
}
