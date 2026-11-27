using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InspekcijskeKontrole.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InspekcijskaTijela",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Inspektorat = table.Column<int>(type: "INTEGER", nullable: false),
                    Nadleznost = table.Column<int>(type: "INTEGER", nullable: false),
                    KontaktOsoba = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspekcijskaTijela", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proizvodi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Proizvodjac = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SerijskiBroj = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ZemljaPorijekla = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Opis = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proizvodi", x => x.Id);
                    table.CheckConstraint("CK_Proizvod_SerijskiBroj", "[SerijskiBroj] IS NULL OR length(trim([SerijskiBroj])) > 0");
                });

            migrationBuilder.CreateTable(
                name: "InspekcijskeKontrole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DatumKontrole = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InspekcijskoTijeloId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProizvodId = table.Column<int>(type: "INTEGER", nullable: false),
                    RezultatiKontrole = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    ProizvodSiguran = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspekcijskeKontrole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspekcijskeKontrole_InspekcijskaTijela_InspekcijskoTijeloId",
                        column: x => x.InspekcijskoTijeloId,
                        principalTable: "InspekcijskaTijela",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspekcijskeKontrole_Proizvodi_ProizvodId",
                        column: x => x.ProizvodId,
                        principalTable: "Proizvodi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InspekcijskaKontrola_Tijelo_Datum",
                table: "InspekcijskeKontrole",
                columns: new[] { "InspekcijskoTijeloId", "DatumKontrole" });

            migrationBuilder.CreateIndex(
                name: "IX_InspekcijskeKontrole_ProizvodId",
                table: "InspekcijskeKontrole",
                column: "ProizvodId");

            migrationBuilder.CreateIndex(
                name: "UX_Proizvod_SerijskiBroj",
                table: "Proizvodi",
                column: "SerijskiBroj",
                unique: true,
                filter: "[SerijskiBroj] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InspekcijskeKontrole");

            migrationBuilder.DropTable(
                name: "InspekcijskaTijela");

            migrationBuilder.DropTable(
                name: "Proizvodi");
        }
    }
}
