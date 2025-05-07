using FluentValidation;
using VMTS.API.Dtos.Vehicles.Model;

namespace VMTS.API.Validators;

public class VehicleModelValidator : AbstractValidator<VehicleModelCreateDto>
{
    public VehicleModelValidator()
    {
        RuleFor(vm => vm.Name).NotEmpty().WithMessage("Model Name Cannto Be Empty");
        RuleFor(vm => vm.CategoryId).NotEmpty().WithMessage("Category Model Cannot Be Empty");
        RuleFor(vm => vm.Manufacturer).NotEmpty().WithMessage("Manufacturer Cannto Be Empty");
        RuleFor(vm => vm.FuelEfficiency).NotEmpty().WithMessage("Fuel Efficiency Cannot Be Empty");
    }
}
