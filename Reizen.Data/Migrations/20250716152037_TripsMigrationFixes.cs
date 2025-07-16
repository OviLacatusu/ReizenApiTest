using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reizen.Data.Migrations
{
    /// <inheritdoc />
    public partial class TripsMigrationFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "klantid",
                table: "bookings",
                newName: "clientid");

            migrationBuilder.RenameColumn(
                name: "geboektOp",
                table: "bookings",
                newName: "bookedondate");

            migrationBuilder.RenameColumn(
                name: "annulatieVerzekering",
                table: "bookings",
                newName: "cancellationInsurance");

            migrationBuilder.RenameIndex(
                name: "IX_bookings_klantid",
                table: "bookings",
                newName: "IX_bookings_clientid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "clientid",
                table: "bookings",
                newName: "klantid");

            migrationBuilder.RenameColumn(
                name: "bookedondate",
                table: "bookings",
                newName: "geboektOp");

            migrationBuilder.RenameColumn(
                name: "cancellationInsurance",
                table: "bookings",
                newName: "annulatieVerzekering");

            migrationBuilder.RenameIndex(
                name: "IX_bookings_clientid",
                table: "bookings",
                newName: "IX_bookings_klantid");
        }
    }
}
