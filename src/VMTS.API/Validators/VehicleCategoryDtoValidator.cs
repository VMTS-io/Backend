using FluentValidation;
using VMTS.API.Dtos.Vehicles.Category;

namespace VMTS.API.Validators;

public class VehicleCategoryDtoValidator : AbstractValidator<VehicleCategoryCreateDto>
{
    public VehicleCategoryDtoValidator()
    {
        RuleFor(vc => vc.Name).NotEmpty().WithMessage("Category Name Cannot Be Empty");
        RuleFor(vc => vc.Description)
            .NotEmpty()
            .WithMessage("Category Description Cannot Be Empty");
    }
}
