using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scheduling_Simulator.Migrations
{
    /// <inheritdoc />
    public partial class newdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    Quiz_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Choice1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Choice2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Choice3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.Quiz_Id);
                });

            migrationBuilder.CreateTable(
                name: "Simulations",
                columns: table => new
                {
                    S_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Algorithm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumProcesses = table.Column<int>(type: "int", nullable: false),
                    QuantumTime = table.Column<int>(type: "int", nullable: true),
                    IsAnswered = table.Column<bool>(type: "bit", nullable: false),
                    S_Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulations", x => x.S_Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "SimulationInputs",
                columns: table => new
                {
                    S_Id = table.Column<int>(type: "int", nullable: false),
                    PId = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<int>(type: "int", nullable: false),
                    BurstTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationInputs", x => new { x.S_Id, x.PId });
                    table.ForeignKey(
                        name: "FK_SimulationInputs_Simulations_S_Id",
                        column: x => x.S_Id,
                        principalTable: "Simulations",
                        principalColumn: "S_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimulationOutput",
                columns: table => new
                {
                    S_Id = table.Column<int>(type: "int", nullable: false),
                    PId = table.Column<int>(type: "int", nullable: false),
                    P_Id = table.Column<int>(type: "int", nullable: true),
                    CompletionTime = table.Column<int>(type: "int", nullable: false),
                    TurnAroundTime = table.Column<int>(type: "int", nullable: false),
                    WaitTime = table.Column<int>(type: "int", nullable: false),
                    ByUser = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationOutput", x => new { x.S_Id, x.PId });
                    table.ForeignKey(
                        name: "FK_SimulationOutput_Simulations_P_Id",
                        column: x => x.P_Id,
                        principalTable: "Simulations",
                        principalColumn: "S_Id");
                    table.ForeignKey(
                        name: "FK_SimulationOutput_Simulations_S_Id",
                        column: x => x.S_Id,
                        principalTable: "Simulations",
                        principalColumn: "S_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Quiz_Id = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => new { x.UserId, x.Quiz_Id });
                    table.ForeignKey(
                        name: "FK_Results_Quiz_Quiz_Id",
                        column: x => x.Quiz_Id,
                        principalTable: "Quiz",
                        principalColumn: "Quiz_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Results_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Results_Quiz_Id",
                table: "Results",
                column: "Quiz_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationOutput_P_Id",
                table: "SimulationOutput",
                column: "P_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "SimulationInputs");

            migrationBuilder.DropTable(
                name: "SimulationOutput");

            migrationBuilder.DropTable(
                name: "Quiz");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Simulations");
        }
    }
}
