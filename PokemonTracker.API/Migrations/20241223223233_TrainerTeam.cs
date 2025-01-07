using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class TrainerTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pokemon_Trainers_TrainerId",
                table: "Pokemon");

            migrationBuilder.RenameColumn(
                name: "TrainerId",
                table: "Pokemon",
                newName: "TrainerID");

            migrationBuilder.RenameIndex(
                name: "IX_Pokemon_TrainerId",
                table: "Pokemon",
                newName: "IX_Pokemon_TrainerID");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerID",
                table: "Pokemon",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pokemon_Trainers_TrainerID",
                table: "Pokemon",
                column: "TrainerID",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pokemon_Trainers_TrainerID",
                table: "Pokemon");

            migrationBuilder.RenameColumn(
                name: "TrainerID",
                table: "Pokemon",
                newName: "TrainerId");

            migrationBuilder.RenameIndex(
                name: "IX_Pokemon_TrainerID",
                table: "Pokemon",
                newName: "IX_Pokemon_TrainerId");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "Pokemon",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Pokemon_Trainers_TrainerId",
                table: "Pokemon",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");
        }
    }
}
