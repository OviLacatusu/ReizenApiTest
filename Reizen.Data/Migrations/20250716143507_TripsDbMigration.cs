using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reizen.Data.Migrations
{
    /// <inheritdoc />
    public partial class TripsDbMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "continents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__continents__3213E83FCC03D392", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "residences",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postalcode = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__residences__3213E83F42709BCC", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Continentid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Countries__3213E83FF994C39E", x => x.id);
                    table.ForeignKey(
                        name: "Countries_continents",
                        column: x => x.Continentid,
                        principalTable: "continents",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    familyname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    firstname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    address = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    residenceid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__clients__3213E83F8BE1A164", x => x.id);
                    table.ForeignKey(
                        name: "clients_residences",
                        column: x => x.residenceid,
                        principalTable: "residences",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "destinations",
                columns: table => new
                {
                    code = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    placename = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Destinations__357D4CF8ABF0313B", x => x.code);
                    table.ForeignKey(
                        name: "Destinations_Countries",
                        column: x => x.CountryId,
                        principalTable: "countries",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "trips",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinationCode = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    dateofdeparture = table.Column<DateOnly>(type: "date", nullable: false),
                    numberOfDays = table.Column<int>(type: "int", nullable: false),
                    pricePerPerson = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    numberOfAdults = table.Column<int>(type: "int", nullable: false),
                    numberOfMinors = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__trips__3213E83F18B8A173", x => x.id);
                    table.ForeignKey(
                        name: "trips_Destinations",
                        column: x => x.DestinationCode,
                        principalTable: "destinations",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    klantid = table.Column<int>(type: "int", nullable: false),
                    tripid = table.Column<int>(type: "int", nullable: false),
                    geboektOp = table.Column<DateOnly>(type: "date", nullable: false),
                    numberOfAdults = table.Column<int>(type: "int", nullable: true),
                    numberOfMinors = table.Column<int>(type: "int", nullable: true),
                    annulatieVerzekering = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__bookings__3213E83FE0274BF8", x => x.id);
                    table.ForeignKey(
                        name: "bookings_clients",
                        column: x => x.klantid,
                        principalTable: "clients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "bookings_trips",
                        column: x => x.tripid,
                        principalTable: "trips",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_klantid",
                table: "bookings",
                column: "klantid");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_tripid",
                table: "bookings",
                column: "tripid");

            migrationBuilder.CreateIndex(
                name: "IX_clients_residenceid",
                table: "clients",
                column: "residenceid");

            migrationBuilder.CreateIndex(
                name: "UQ__continents__72E1CD7890D795ED",
                table: "continents",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_countries_Continentid",
                table: "countries",
                column: "Continentid");

            migrationBuilder.CreateIndex(
                name: "UQ__Countries__72E1CD78CA87044C",
                table: "countries",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_destinations_CountryId",
                table: "destinations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_trips_DestinationCode",
                table: "trips",
                column: "DestinationCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "trips");

            migrationBuilder.DropTable(
                name: "residences");

            migrationBuilder.DropTable(
                name: "destinations");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "continents");

            migrationBuilder.CreateTable(
                name: "werelddelen",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naam = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__werelddelen__3213E83FCC03D392", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "woonplaatsen",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naam = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    postcode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__woonplaa__3213E83F42709BCC", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "landen",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    werelddeelid = table.Column<int>(type: "int", nullable: false),
                    naam = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__landen__3213E83FF994C39E", x => x.id);
                    table.ForeignKey(
                        name: "landen_werelddelen",
                        column: x => x.werelddeelid,
                        principalTable: "werelddelen",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "klanten",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    woonplaatsid = table.Column<int>(type: "int", nullable: false),
                    adres = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    familienaam = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    voornaam = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__klanten__3213E83F8BE1A164", x => x.id);
                    table.ForeignKey(
                        name: "klanten_woonplaatsen",
                        column: x => x.woonplaatsid,
                        principalTable: "woonplaatsen",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "bestemmingen",
                columns: table => new
                {
                    code = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    landid = table.Column<int>(type: "int", nullable: false),
                    plaats = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__bestemmingen__357D4CF8ABF0313B", x => x.code);
                    table.ForeignKey(
                        name: "bestemmingen_landen",
                        column: x => x.landid,
                        principalTable: "landen",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "reizen",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bestemmingscode = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    aantalDagen = table.Column<int>(type: "int", nullable: false),
                    aantalKinderen = table.Column<int>(type: "int", nullable: false),
                    aantalVolwassenen = table.Column<int>(type: "int", nullable: false),
                    prijsPerPersoon = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    vertrek = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__reizen__3213E83F18B8A173", x => x.id);
                    table.ForeignKey(
                        name: "reizen_bestemmingen",
                        column: x => x.bestemmingscode,
                        principalTable: "bestemmingen",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "boekingen",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    klantid = table.Column<int>(type: "int", nullable: false),
                    reisid = table.Column<int>(type: "int", nullable: false),
                    aantalKinderen = table.Column<int>(type: "int", nullable: true),
                    aantalVolwassenen = table.Column<int>(type: "int", nullable: true),
                    annulatieVerzekering = table.Column<bool>(type: "bit", nullable: false),
                    geboektOp = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__boekingen__3213E83FE0274BF8", x => x.id);
                    table.ForeignKey(
                        name: "boekingen_klanten",
                        column: x => x.klantid,
                        principalTable: "klanten",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "boekingen_reizen",
                        column: x => x.reisid,
                        principalTable: "reizen",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bestemmingen_landid",
                table: "bestemmingen",
                column: "landid");

            migrationBuilder.CreateIndex(
                name: "IX_boekingen_klantid",
                table: "boekingen",
                column: "klantid");

            migrationBuilder.CreateIndex(
                name: "IX_boekingen_reisid",
                table: "boekingen",
                column: "reisid");

            migrationBuilder.CreateIndex(
                name: "IX_klanten_woonplaatsid",
                table: "klanten",
                column: "woonplaatsid");

            migrationBuilder.CreateIndex(
                name: "IX_landen_werelddeelid",
                table: "landen",
                column: "werelddeelid");

            migrationBuilder.CreateIndex(
                name: "UQ__landen__72E1CD78CA87044C",
                table: "landen",
                column: "naam",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reizen_bestemmingscode",
                table: "reizen",
                column: "bestemmingscode");

            migrationBuilder.CreateIndex(
                name: "UQ__wereldde__72E1CD7890D795ED",
                table: "werelddelen",
                column: "naam",
                unique: true);
        }
    }
}
