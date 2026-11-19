using Application.Abstractions.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Registrations.Get;

public sealed class GetRegistrationByIdHandler : IRequestHandler<GetRegistrationByIdQuery, RegistrationListItem?>
{
    private readonly IAppDbContext _dbContext;

    public GetRegistrationByIdHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RegistrationListItem?> Handle(
        GetRegistrationByIdQuery query,
        CancellationToken cancellationToken)
    {
        var registration = await _dbContext.Registrations
            .AsNoTracking()
            .FirstOrDefaultAsync(registration => registration.Id == query.Id, cancellationToken);

        return registration is null
            ? null
            : new RegistrationListItem(
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
                registration.PaymentStatus);
    }
}
