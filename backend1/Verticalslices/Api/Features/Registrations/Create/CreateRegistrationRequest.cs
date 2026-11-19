using Domain.Enums;

namespace Api.Features.Registrations.Create;

public sealed record CreateRegistrationRequest(
    string ParticipantName,
    string ContactNumber,
    string Email,
    string Address,
    string CarMakeModel,
    string PlateNumber,
    RegistrationType RegistrationType);
