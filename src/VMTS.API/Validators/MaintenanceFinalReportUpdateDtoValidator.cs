using FluentValidation;
using VMTS.API.Dtos.Maintenance.Report.Final;

namespace VMTS.API.Validators;

public class MaintenanceFinalReportUpdateDtoValidator
    : AbstractValidator<MaintenanceFinalReportUpdateDto>
{
    public MaintenanceFinalReportUpdateDtoValidator()
    {
        RuleFor(x => x.Notes)
            // .NotEmpty()
            // .WithMessage("Notes are required.")
            .MaximumLength(1000)
            .WithMessage("Notes must not exceed 1000 characters.");

        RuleFor(x => x.ChangedParts).NotNull().WithMessage("Changed parts list is required.");
        // .Must(p => p.Count > 0)
        // .WithMessage("At least one changed part must be specified.");

        RuleForEach(x => x.ChangedParts).SetValidator(new MaintenanceReportPartDtoValidator());
    }
}
