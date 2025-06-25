using FluentValidation;
using VMTS.API.Dtos.Maintenance.Report;

namespace VMTS.API.Validators
{
    public class MaintenanceReportPartDtoValidator : AbstractValidator<MaintenanceReportPartDto>
    {
        public MaintenanceReportPartDtoValidator()
        {
            RuleFor(x => x.PartId).NotEmpty().WithMessage("PartId is required.");

            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}

