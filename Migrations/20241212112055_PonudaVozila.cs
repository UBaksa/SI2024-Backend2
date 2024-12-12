using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace carGooBackend.Migrations
{
    /// <inheritdoc />
    public partial class PonudaVozila : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Preduzeca_PreduzeceId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "PonudaVozila",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrzavaU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrzavaI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MestoU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MestoI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Utovar = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Istovar = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duzina = table.Column<double>(type: "float", nullable: false),
                    Tezina = table.Column<double>(type: "float", nullable: false),
                    TipNadogradnje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipKamiona = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdPreduzeca = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdKorisnika = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Vreme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PonudaVozila", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PonudaVozila_AspNetUsers_IdKorisnika",
                        column: x => x.IdKorisnika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PonudaVozila_Preduzeca_IdPreduzeca",
                        column: x => x.IdPreduzeca,
                        principalTable: "Preduzeca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PonudaVozila_IdKorisnika",
                table: "PonudaVozila",
                column: "IdKorisnika");

            migrationBuilder.CreateIndex(
                name: "IX_PonudaVozila_IdPreduzeca",
                table: "PonudaVozila",
                column: "IdPreduzeca");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Preduzeca_PreduzeceId",
                table: "AspNetUsers",
                column: "PreduzeceId",
                principalTable: "Preduzeca",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Preduzeca_PreduzeceId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PonudaVozila");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Preduzeca_PreduzeceId",
                table: "AspNetUsers",
                column: "PreduzeceId",
                principalTable: "Preduzeca",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
