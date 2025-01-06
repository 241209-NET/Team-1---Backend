using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class PkmnnDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pkmns",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Pkmns_Name",
                table: "Pkmns",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pkmns_Name",
                table: "Pkmns");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pkmns",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
