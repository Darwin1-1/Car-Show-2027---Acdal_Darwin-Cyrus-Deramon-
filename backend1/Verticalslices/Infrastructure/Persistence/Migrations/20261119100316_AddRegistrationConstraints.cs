using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Registrations_AmountPaid",
                table: "Registrations",
                sql: "\"AmountPaid\" >= 0 AND \"AmountPaid\" <= \"RegistrationFee\"");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Registrations_RegistrationFee",
                table: "Registrations",
                sql: "\"RegistrationFee\" IN (1500, 2500, 5000)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Registrations_AmountPaid",
                table: "Registrations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Registrations_RegistrationFee",
                table: "Registrations");
        }
    }
}
