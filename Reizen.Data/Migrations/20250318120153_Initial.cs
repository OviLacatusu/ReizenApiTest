using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reizen.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "boekingen");

            migrationBuilder.DropTable(
                name: "klanten");

            migrationBuilder.DropTable(
                name: "reizen");

            migrationBuilder.DropTable(
                name: "woonplaatsen");

            migrationBuilder.DropTable(
                name: "bestemmingen");

            migrationBuilder.DropTable(
                name: "landen");

            migrationBuilder.DropTable(
                name: "werelddelen");
        }
    }
}
