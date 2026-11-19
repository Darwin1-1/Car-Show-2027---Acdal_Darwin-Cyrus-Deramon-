using MediatR;

namespace Application.Features.Registrations.Get;

public sealed record GetRegistrationsQuery(
    string? ParticipantName = null,
    string? Type = null,
    string? Status = null,
    int Page = 1,
    int PageSize = 10) : IRequest<GetRegistrationsResponse>;
