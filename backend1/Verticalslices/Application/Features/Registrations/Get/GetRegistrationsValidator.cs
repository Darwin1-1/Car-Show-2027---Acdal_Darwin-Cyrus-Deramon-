using Domain.Enums;
using FluentValidation;

namespace Application.Features.Registrations.Get;

public sealed class GetRegistrationsValidator : AbstractValidator<GetRegistrationsQuery>
{
    public GetRegistrationsValidator()
    {
        RuleFor(query => query.ParticipantName)
            .MaximumLength(200);

        RuleFor(query => query.Type)
            .Must(BeRegistrationType)
            .When(query => !string.IsNullOrWhiteSpace(query.Type))
            .WithMessage("Type must be Exhibitor, Competitor, or VIP.");

        RuleFor(query => query.Status)
            .Must(BeRegistrationStatus)
            .When(query => !string.IsNullOrWhiteSpace(query.Status))
            .WithMessage("Status must be Pending, Confirmed, CheckedIn, or Cancelled.");

        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 50);
    }

    private static bool BeRegistrationType(string? value) =>
        Enum.TryParse<RegistrationType>(value, true, out var parsed)
        && Enum.IsDefined(parsed)
        && !int.TryParse(value, out _);

    private static bool BeRegistrationStatus(string? value) =>
        Enum.TryParse<RegistrationStatus>(value, true, out var parsed)
        && Enum.IsDefined(parsed)
        && !int.TryParse(value, out _);
}
