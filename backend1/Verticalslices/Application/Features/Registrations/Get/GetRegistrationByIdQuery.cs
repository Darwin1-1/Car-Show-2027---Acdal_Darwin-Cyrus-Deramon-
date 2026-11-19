using MediatR;

namespace Application.Features.Registrations.Get;

public sealed record GetRegistrationByIdQuery(Guid Id) : IRequest<RegistrationListItem?>;
