using FluentValidation;
using VMTS.API.Dtos.Maintenance.Report.Initial;

namespace VMTS.API.Validators
{
    public class MaintenanceInitialReportUpdateValidator
        : AbstractValidator<MaintenanceInitialReportUpdateDto>
    {
        public MaintenanceInitialReportUpdateValidator()
        {
            RuleFor(x => x.Notes)
                // .NotEmpty()
                // .WithMessage("Notes are required.")
                .MaximumLength(1000)
                .WithMessage("Notes must not exceed 1000 characters.");

            RuleFor(x => x.ExpectedFinishDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.Date))
                .WithMessage("Expected finish date cannot be in the past.");

            RuleFor(x => x.ExpectedChangedParts)
                .NotNull()
                .WithMessage("ExpectedChangedParts list is required.");

            RuleForEach(x => x.ExpectedChangedParts)
                .SetValidator(new MaintenanceReportPartDtoValidator());
        }
    }
}

