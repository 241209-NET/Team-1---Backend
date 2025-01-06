using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class TrainersData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumTeamMembers",
                table: "Trainers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumTeamMembers",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
