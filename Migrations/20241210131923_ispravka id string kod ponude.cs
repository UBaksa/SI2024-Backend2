using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace carGooBackend.Migrations
{
    /// <inheritdoc />
    public partial class ispravkaidstringkodponude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ponude_AspNetUsers_KorisnikId",
                table: "Ponude");

            migrationBuilder.DropForeignKey(
                name: "FK_Ponude_Preduzeca_PreduzeceId",
                table: "Ponude");

            migrationBuilder.DropIndex(
                name: "IX_Ponude_KorisnikId",
                table: "Ponude");

            migrationBuilder.DropIndex(
                name: "IX_Ponude_PreduzeceId",
                table: "Ponude");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "Ponude");

            migrationBuilder.DropColumn(
                name: "PreduzeceId",
                table: "Ponude");

            migrationBuilder.AlterColumn<string>(
                name: "IdKorisnika",
                table: "Ponude",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Ponude_IdKorisnika",
                table: "Ponude",
                column: "IdKorisnika");

            migrationBuilder.CreateIndex(
                name: "IX_Ponude_IdPreduzeca",
                table: "Ponude",
                column: "IdPreduzeca");

            migrationBuilder.AddForeignKey(
                name: "FK_Ponude_AspNetUsers_IdKorisnika",
                table: "Ponude",
                column: "IdKorisnika",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ponude_Preduzeca_IdPreduzeca",
                table: "Ponude",
                column: "IdPreduzeca",
                principalTable: "Preduzeca",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ponude_AspNetUsers_IdKorisnika",
                table: "Ponude");

            migrationBuilder.DropForeignKey(
                name: "FK_Ponude_Preduzeca_IdPreduzeca",
                table: "Ponude");

            migrationBuilder.DropIndex(
                name: "IX_Ponude_IdKorisnika",
                table: "Ponude");

            migrationBuilder.DropIndex(
                name: "IX_Ponude_IdPreduzeca",
                table: "Ponude");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdKorisnika",
                table: "Ponude",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "KorisnikId",
                table: "Ponude",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PreduzeceId",
                table: "Ponude",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ponude_KorisnikId",
                table: "Ponude",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Ponude_PreduzeceId",
                table: "Ponude",
                column: "PreduzeceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ponude_AspNetUsers_KorisnikId",
                table: "Ponude",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ponude_Preduzeca_PreduzeceId",
                table: "Ponude",
                column: "PreduzeceId",
                principalTable: "Preduzeca",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
