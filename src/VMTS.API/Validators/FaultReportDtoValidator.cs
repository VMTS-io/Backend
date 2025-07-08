using FluentValidation;
using VMTS.API.Dtos;

namespace VMTS.API.Validators;

public class FaultReportDtoValidator : AbstractValidator<FaultReportRequest>
{
    public FaultReportDtoValidator()
    {
        RuleFor(fr => fr.Details).NotEmpty().WithMessage("Details Required");
        RuleFor(fr => fr.FuelRefile).NotEmpty().WithMessage("FuelRefile Required");
        RuleFor(fr => fr.Cost).NotEmpty().WithMessage("Cost Required");
        RuleFor(fr => fr.Address).NotEmpty().WithMessage("Address Required");
    }
}
