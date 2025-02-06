using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace carGooBackend.Migrations
{
    /// <inheritdoc />
    public partial class NazivMigracije : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Languages",
                table: "AspNetUsers");
        }
    }
}
