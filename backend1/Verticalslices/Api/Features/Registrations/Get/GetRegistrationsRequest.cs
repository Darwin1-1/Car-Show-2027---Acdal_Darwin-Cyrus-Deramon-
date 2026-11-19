namespace Api.Features.Registrations.Get;

public sealed record GetRegistrationsRequest(
    string? ParticipantName = null,
    string? Type = null,
    string? Status = null,
    int Page = 1,
    int PageSize = 10);
