using FluentValidation;
using PpsrRegistration.Models;

public class VehicleRegistrationValidator : AbstractValidator<VehicleRegistration>
{
    public VehicleRegistrationValidator()
    {
        RuleFor(x => x.GrantorFirstName).NotEmpty().MaximumLength(35);
        RuleFor(x => x.GrantorMiddleNames).MaximumLength(75);
        RuleFor(x => x.GrantorLastName).NotEmpty().MaximumLength(35);
        RuleFor(x => x.VIN).NotEmpty().Length(17);
        RuleFor(x => x.SPG_ACN).Matches(@"^\d{9}$");
        RuleFor(x => x.SPG_OrgName).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Duration).Must(v => new[] { "7", "25", "N/A" }.Contains(v));
    }
}
