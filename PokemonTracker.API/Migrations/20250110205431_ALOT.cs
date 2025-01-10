using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class ALOT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trainers_Name",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Pkmns_Name",
                table: "Pkmns");

            migrationBuilder.DropIndex(
                name: "IX_Pkmns_TrainerID",
                table: "Pkmns");

            migrationBuilder.DropColumn(
                name: "PokedexDesc",
                table: "Pkmns");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Pkmns");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Trainers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Username",
                table: "Trainers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pkmns_TrainerID_Name",
                table: "Pkmns",
                columns: new[] { "TrainerID", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trainers_Username",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Pkmns_TrainerID_Name",
                table: "Pkmns");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Trainers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Trainers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PokedexDesc",
                table: "Pkmns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Pkmns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Name",
                table: "Trainers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pkmns_Name",
                table: "Pkmns",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pkmns_TrainerID",
                table: "Pkmns",
                column: "TrainerID");
        }
    }
}
