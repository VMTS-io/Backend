using FluentValidation;
using VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;

namespace VMTS.API.Validators;

public class VehicleTrackingValidator : AbstractValidator<VehicleWithDuePartsSpecParams>
{
    public VehicleTrackingValidator()
    {
        RuleFor(sp => sp.VehicleId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Vehicle Id Cannot Be Null or Empty");
    }
}
