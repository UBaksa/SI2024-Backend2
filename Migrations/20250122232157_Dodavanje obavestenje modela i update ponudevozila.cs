using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace carGooBackend.Migrations
{
    /// <inheritdoc />
    public partial class Dodavanjeobavestenjemodelaiupdateponudevozila : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RadiusI",
                table: "PonudaVozila",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RadiusI",
                table: "PonudaVozila");
        }
    }
}
