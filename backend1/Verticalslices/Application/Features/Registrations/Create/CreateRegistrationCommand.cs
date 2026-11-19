using Domain.Enums;
using MediatR;

namespace Application.Features.Registrations.Create;

public sealed record CreateRegistrationCommand(
    string ParticipantName,
    string ContactNumber,
    string Email,
    string Address,
    string CarMakeModel,
    string PlateNumber,
    RegistrationType RegistrationType) : IRequest<Common.Results.Result<CreateRegistrationResponse>>;
