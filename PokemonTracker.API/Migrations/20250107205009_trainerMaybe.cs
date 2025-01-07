using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class trainerMaybe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pkmns_Trainers_trainerId",
                table: "Pkmns");

            migrationBuilder.RenameColumn(
                name: "trainerId",
                table: "Pkmns",
                newName: "TrainerID");

            migrationBuilder.RenameIndex(
                name: "IX_Pkmns_trainerId",
                table: "Pkmns",
                newName: "IX_Pkmns_TrainerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pkmns_Trainers_TrainerID",
                table: "Pkmns",
                column: "TrainerID",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pkmns_Trainers_TrainerID",
                table: "Pkmns");

            migrationBuilder.RenameColumn(
                name: "TrainerID",
                table: "Pkmns",
                newName: "trainerId");

            migrationBuilder.RenameIndex(
                name: "IX_Pkmns_TrainerID",
                table: "Pkmns",
                newName: "IX_Pkmns_trainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pkmns_Trainers_trainerId",
                table: "Pkmns",
                column: "trainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
