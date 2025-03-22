using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebshopBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelFromScript_20250322_140819 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_ArtNr",
                table: "Products",
                column: "ArtNr",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ArtNr",
                table: "Products");
        }
    }
}
