using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scheduling_Simulator.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Simulations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Simulations_UserId",
                table: "Simulations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Simulations_User_UserId",
                table: "Simulations",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Simulations_User_UserId",
                table: "Simulations");

            migrationBuilder.DropIndex(
                name: "IX_Simulations_UserId",
                table: "Simulations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Simulations");
        }
    }
}
