using FluentValidation;
using Domain.Entities;

namespace Application.Features.Registrations.Create;

public sealed class CreateRegistrationValidator : AbstractValidator<CreateRegistrationCommand>
{
    public CreateRegistrationValidator()
    {
        RuleFor(command => command.ParticipantName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.ContactNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(7, 30)
            .Matches("^[0-9+() -]+$");

        RuleFor(command => command.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(320)
            .EmailAddress();

        RuleFor(command => command.Address)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(command => command.CarMakeModel)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.PlateNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(20)
            .Matches("^[A-Za-z0-9 -]+$");

        RuleFor(command => command.RegistrationType)
            .IsInEnum();
    }
}
