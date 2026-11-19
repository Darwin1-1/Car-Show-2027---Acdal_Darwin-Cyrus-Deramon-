using Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<global::Domain.Entities.Registration> Registrations => Set<global::Domain.Entities.Registration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<global::Domain.Entities.Registration>(entity =>
        {
            entity.ToTable("Registrations", table =>
            {
                table.HasCheckConstraint(
                    "CK_Registrations_AmountPaid",
                    "\"AmountPaid\" >= 0 AND \"AmountPaid\" <= \"RegistrationFee\"");

                table.HasCheckConstraint(
                    "CK_Registrations_RegistrationFee",
                    "\"RegistrationFee\" IN (1500, 2500, 5000)");
            });
            entity.HasKey(registration => registration.Id);

            entity.Property(registration => registration.Id)
                .ValueGeneratedNever();

            entity.Property(registration => registration.RegistrationNumber)
                .IsRequired()
                .HasMaxLength(9);

            entity.Property(registration => registration.ParticipantName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(registration => registration.ContactNumber)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(registration => registration.Email)
                .IsRequired()
                .HasMaxLength(320);

            entity.Property(registration => registration.Address)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(registration => registration.CarMakeModel)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(registration => registration.PlateNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(registration => registration.RegistrationFee)
                .HasPrecision(18, 2);

            entity.Property(registration => registration.AmountPaid)
                .HasPrecision(18, 2);

            entity.Property(registration => registration.RegistrationType)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(registration => registration.Status)
                .HasConversion<string>()
                .IsRequired();

            entity.HasIndex(registration => registration.RegistrationNumber)
                .IsUnique()
                .HasDatabaseName("UX_Registrations_RegistrationNumber");

            entity.HasIndex(registration => registration.PlateNumber)
                .IsUnique()
                .HasDatabaseName("UX_Registrations_PlateNumber");

            entity.Ignore(registration => registration.PaymentStatus);
        });
    }
}
