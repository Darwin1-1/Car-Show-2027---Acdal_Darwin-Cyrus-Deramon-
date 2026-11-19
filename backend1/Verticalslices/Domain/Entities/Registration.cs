using Domain.Enums;
using System.Text.RegularExpressions;

namespace Domain.Entities;

public sealed class Registration
{
    private static readonly Regex RegistrationNumberPattern =
        new("^CS27-[0-9]{4}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public Guid Id { get; private set; }
    public string RegistrationNumber { get; private set; } = string.Empty;
    public DateTime RegistrationDate { get; private set; }
    public string ParticipantName { get; private set; } = string.Empty;
    public string ContactNumber { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string CarMakeModel { get; private set; } = string.Empty;
    public string PlateNumber { get; private set; } = string.Empty;
    public RegistrationType RegistrationType { get; private set; }
    public RegistrationStatus Status { get; private set; }
    public decimal RegistrationFee { get; private set; }
    public decimal AmountPaid { get; private set; }

    public PaymentStatus PaymentStatus => AmountPaid switch
    {
        0 => PaymentStatus.Unpaid,
        _ when AmountPaid == RegistrationFee => PaymentStatus.Paid,
        _ => PaymentStatus.Partial
    };

    private Registration()
    {
    }

    public Registration(
        Guid id,
        string registrationNumber,
        DateTime registrationDate,
        string participantName,
        string contactNumber,
        string email,
        string address,
        string carMakeModel,
        string plateNumber,
        RegistrationType registrationType,
        decimal amountPaid = 0)
    {
        Id = id == Guid.Empty ? throw new ArgumentException("Id cannot be empty.", nameof(id)) : id;

        SetRegistrationNumber(registrationNumber);
        SetRegistrationDate(registrationDate);
        SetParticipantName(participantName);
        SetContactNumber(contactNumber);
        SetEmail(email);
        SetAddress(address);
        SetCarMakeModel(carMakeModel);
        SetPlateNumber(plateNumber);

        if (!Enum.IsDefined(registrationType))
        {
            throw new ArgumentOutOfRangeException(nameof(registrationType), "The registration type is invalid.");
        }

        RegistrationType = registrationType;
        RegistrationFee = GetFee(registrationType);
        Status = RegistrationStatus.Pending;
        SetAmountPaid(amountPaid);
    }

    public static Registration Create(
        int sequence,
        string participantName,
        string contactNumber,
        string email,
        string address,
        string carMakeModel,
        string plateNumber,
        RegistrationType registrationType,
        decimal amountPaid = 0,
        DateTime? registrationDate = null)
    {
        if (sequence is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(sequence), "Sequence must be between 1 and 9999.");
        }

        return new Registration(
            Guid.NewGuid(),
            GenerateRegistrationNumber(sequence),
            registrationDate ?? DateTime.UtcNow,
            participantName,
            contactNumber,
            email,
            address,
            carMakeModel,
            plateNumber,
            registrationType,
            amountPaid);
    }

    public static string GenerateRegistrationNumber(int sequence)
    {
        if (sequence is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(sequence), "Sequence must be between 1 and 9999.");
        }

        return $"CS27-{sequence:D4}";
    }

    public static decimal GetFee(RegistrationType registrationType) => registrationType switch
    {
        RegistrationType.Exhibitor => 1500m,
        RegistrationType.Competitor => 2500m,
        RegistrationType.VIP => 5000m,
        _ => throw new ArgumentOutOfRangeException(nameof(registrationType), "The registration type is invalid.")
    };

    public void SetAmountPaid(decimal amountPaid)
    {
        if (amountPaid < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountPaid), "Amount paid cannot be negative.");
        }

        if (amountPaid > RegistrationFee)
        {
            throw new ArgumentOutOfRangeException(nameof(amountPaid), "Amount paid cannot exceed the registration fee.");
        }

        AmountPaid = amountPaid;
    }

    public void SetStatus(RegistrationStatus status)
    {
        if (!Enum.IsDefined(status))
        {
            throw new ArgumentOutOfRangeException(nameof(status), "The registration status is invalid.");
        }

        Status = status;
    }

    public void SetPlateNumber(string plateNumber)
    {
        PlateNumber = NormalizeRequiredText(plateNumber, nameof(plateNumber));
    }

    private void SetRegistrationNumber(string registrationNumber)
    {
        var value = NormalizeRequiredText(registrationNumber, nameof(registrationNumber));
        if (!RegistrationNumberPattern.IsMatch(value))
        {
            throw new ArgumentException("Registration number must follow the CS27-0001 format.", nameof(registrationNumber));
        }

        RegistrationNumber = value;
    }

    private void SetRegistrationDate(DateTime registrationDate)
    {
        if (registrationDate == default)
        {
            throw new ArgumentException("Registration date is required.", nameof(registrationDate));
        }

        RegistrationDate = registrationDate;
    }

    private void SetParticipantName(string participantName) =>
        ParticipantName = NormalizeRequiredText(participantName, nameof(participantName));

    private void SetContactNumber(string contactNumber) =>
        ContactNumber = NormalizeRequiredText(contactNumber, nameof(contactNumber));

    private void SetEmail(string email) =>
        Email = NormalizeRequiredText(email, nameof(email));

    private void SetAddress(string address) =>
        Address = NormalizeRequiredText(address, nameof(address));

    private void SetCarMakeModel(string carMakeModel) =>
        CarMakeModel = NormalizeRequiredText(carMakeModel, nameof(carMakeModel));

    private static string NormalizeRequiredText(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", parameterName);
        }

        return value.Trim();
    }
}
