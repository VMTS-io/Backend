using FluentValidation;
using VMTS.API.Dtos.Vehicles;

namespace VMTS.API.Validators
{
    public class VehicleDtoValidator : AbstractValidator<VehicleUpsertDto>
    {
        public VehicleDtoValidator()
        {
            RuleFor(v => v.PalletNumber).NotEmpty().WithMessage("Pallet Number Cannot Be Empty");

            RuleFor(v => v.ModelId).NotEmpty().WithMessage("Model Cannot Be Empty");

            RuleFor(v => v.ModelYear)
                .NotEmpty()
                .WithMessage("Model Year Cannot Be Empty")
                .InclusiveBetween((short)1980, (short)DateTime.Now.Year)
                .WithMessage($"Vehicle Models Must Be Between 1980 to {DateTime.Now.Year}");

            RuleFor(v => v.JoinedYear)
                .NotEmpty()
                .WithMessage("Joined Year Cannot Be Empty")
                .InclusiveBetween((short)1980, (short)DateTime.Now.Year)
                .WithMessage($"Joined Year Must Be Between 1980 to {DateTime.Now.Year}");
        }
    }
}
