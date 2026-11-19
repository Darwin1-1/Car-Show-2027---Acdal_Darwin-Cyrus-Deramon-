using Application.Abstractions.Data;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Registrations.Get;

public sealed class GetRegistrationsHandler : IRequestHandler<GetRegistrationsQuery, GetRegistrationsResponse>
{
    private readonly IAppDbContext _dbContext;

    public GetRegistrationsHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetRegistrationsResponse> Handle(
        GetRegistrationsQuery query,
        CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.Registration> registrations = _dbContext.Registrations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.ParticipantName))
        {
            var escapedName = EscapeLikePattern(query.ParticipantName.Trim());
            registrations = registrations.Where(registration =>
                EF.Functions.Like(registration.ParticipantName, $"%{escapedName}%", "\\"));
        }

        if (!string.IsNullOrWhiteSpace(query.Type))
        {
            var registrationType = Enum.Parse<RegistrationType>(query.Type, true);
            registrations = registrations.Where(registration => registration.RegistrationType == registrationType);
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            var registrationStatus = Enum.Parse<RegistrationStatus>(query.Status, true);
            registrations = registrations.Where(registration => registration.Status == registrationStatus);
        }

        var totalCount = await registrations.CountAsync(cancellationToken);
        var totalPages = (int)(((long)totalCount + query.PageSize - 1) / query.PageSize);
        var offset = ((long)query.Page - 1) * query.PageSize;

        var registrationsForPage = offset > int.MaxValue
            ? []
            : await registrations
                .OrderByDescending(registration => registration.RegistrationDate)
                .ThenByDescending(registration => registration.Id)
                .Skip((int)offset)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

        var items = registrationsForPage
            .Select(registration => new RegistrationListItem(
                registration.Id,
                registration.RegistrationNumber,
                registration.RegistrationDate,
                registration.ParticipantName,
                registration.ContactNumber,
                registration.Email,
                registration.Address,
                registration.CarMakeModel,
                registration.PlateNumber,
                registration.RegistrationType,
                registration.Status,
                registration.RegistrationFee,
                registration.AmountPaid,
                registration.PaymentStatus))
            .ToArray();

        return new GetRegistrationsResponse(items, query.Page, query.PageSize, totalCount, totalPages);
    }

    private static string EscapeLikePattern(string value) =>
        value.Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("%", "\\%", StringComparison.Ordinal)
            .Replace("_", "\\_", StringComparison.Ordinal);
}
