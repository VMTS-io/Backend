using FluentValidation;
using VMTS.API.Dtos.Maintenance.Request;

namespace VMTS.API.Validators;

public class MaintenanceRequestUpsertDtoValidator : AbstractValidator<MaintenanceRequestUpsertDto>
{
    public MaintenanceRequestUpsertDtoValidator()
    {
        RuleFor(x => x.MechanicId).NotEmpty().WithMessage("Mechanic ID is required.");

        RuleFor(x => x.VehicleId).NotEmpty().WithMessage("Vehicle ID is required.");

        // RuleFor(x => x.MaintenanceCategoryId)
        //     .NotEmpty()
        //     .WithMessage("Maintenance category ID is required.");

        RuleFor(x => x.Description)
            // .NotEmpty()
            // .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
        RuleForEach(x => x.Parts).NotEmpty();
    }
}
