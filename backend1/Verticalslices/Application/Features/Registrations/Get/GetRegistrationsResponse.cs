namespace Application.Features.Registrations.Get;

public sealed record GetRegistrationsResponse(
    IReadOnlyList<RegistrationListItem> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);
