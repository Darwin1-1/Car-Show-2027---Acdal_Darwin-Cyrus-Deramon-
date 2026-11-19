using Application.Abstractions.Data;
using Application.Common.Results;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Registrations.Create;

public sealed class CreateRegistrationHandler
    : IRequestHandler<CreateRegistrationCommand, Result<CreateRegistrationResponse>>
{
    private readonly IAppDbContext _dbContext;

    public CreateRegistrationHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CreateRegistrationResponse>> Handle(
        CreateRegistrationCommand command,
        CancellationToken cancellationToken)
    {
        var plateNumber = command.PlateNumber.Trim().ToUpperInvariant();
        var plateNumberExists = await _dbContext.Registrations
            .AnyAsync(registration => registration.PlateNumber == plateNumber, cancellationToken);

        if (plateNumberExists)
        {
            return Result<CreateRegistrationResponse>.Failure(
                Error.Conflict("Registration.PlateNumberAlreadyExists", "A registration with this plate number already exists."));
        }

        var latestRegistrationNumber = await _dbContext.Registrations
            .OrderByDescending(registration => registration.RegistrationNumber)
            .Select(registration => registration.RegistrationNumber)
            .FirstOrDefaultAsync(cancellationToken);

        var lastSequence = 0;
        if (latestRegistrationNumber is not null
            && (latestRegistrationNumber.Length != 9
                || !latestRegistrationNumber.StartsWith("CS27-", StringComparison.Ordinal)
                || !int.TryParse(latestRegistrationNumber.AsSpan(5), out lastSequence)))
        {
            return Result<CreateRegistrationResponse>.Failure(
                Error.Failure("Registration.InvalidSequence", "The stored registration number sequence is invalid."));
        }

        if (lastSequence >= 9999)
        {
            return Result<CreateRegistrationResponse>.Failure(
                Error.Conflict("Registration.SequenceExhausted", "The registration number sequence is exhausted."));
        }

        var registrationDate = DateTime.UtcNow;
        var registration = Registration.Create(
            lastSequence + 1,
            command.ParticipantName,
            command.ContactNumber,
            command.Email,
            command.Address,
            command.CarMakeModel,
            plateNumber,
            command.RegistrationType,
            registrationDate: registrationDate);

        _dbContext.Registrations.Add(registration);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new CreateRegistrationResponse(
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

        return Result<CreateRegistrationResponse>.Success(response);
    }
}
