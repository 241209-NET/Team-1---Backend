using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class dexNum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DexNumber",
                table: "Pkmns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DexNumber",
                table: "Pkmns");
        }
    }
}
