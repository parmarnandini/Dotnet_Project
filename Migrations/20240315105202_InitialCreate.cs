using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scheduling_Simulator.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OSQuiz",
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
                    table.PrimaryKey("PK_OSQuiz", x => x.Quiz_Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Quiz_Id = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => new { x.UserId, x.Quiz_Id });
                    table.ForeignKey(
                        name: "FK_Result_OSQuiz_Quiz_Id",
                        column: x => x.Quiz_Id,
                        principalTable: "OSQuiz",
                        principalColumn: "Quiz_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Result_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Simulation",
                columns: table => new
                {
                    S_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Algorithm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumProcesses = table.Column<int>(type: "int", nullable: false),
                    QuantumTime = table.Column<int>(type: "int", nullable: true),
                    IsAnswered = table.Column<bool>(type: "bit", nullable: false),
                    S_Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulation", x => x.S_Id);
                    table.ForeignKey(
                        name: "FK_Simulation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimulationInput",
                columns: table => new
                {
                    S_Id = table.Column<int>(type: "int", nullable: false),
                    PId = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<int>(type: "int", nullable: false),
                    BurstTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationInput", x => new { x.S_Id, x.PId });
                    table.ForeignKey(
                        name: "FK_SimulationInput_Simulation_S_Id",
                        column: x => x.S_Id,
                        principalTable: "Simulation",
                        principalColumn: "S_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimulationOutput",
                columns: table => new
                {
                    S_Id = table.Column<int>(type: "int", nullable: false),
                    PId = table.Column<int>(type: "int", nullable: false),
                    CompletionTime = table.Column<int>(type: "int", nullable: false),
                    TurnAroundTime = table.Column<int>(type: "int", nullable: false),
                    WaitTime = table.Column<int>(type: "int", nullable: false),
                    ByUser = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationOutput", x => new { x.S_Id, x.PId });
                    table.ForeignKey(
                        name: "FK_SimulationOutput_Simulation_S_Id",
                        column: x => x.S_Id,
                        principalTable: "Simulation",
                        principalColumn: "S_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Result_Quiz_Id",
                table: "Result",
                column: "Quiz_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Simulation_UserId",
                table: "Simulation",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Result");

            migrationBuilder.DropTable(
                name: "SimulationInput");

            migrationBuilder.DropTable(
                name: "SimulationOutput");

            migrationBuilder.DropTable(
                name: "OSQuiz");

            migrationBuilder.DropTable(
                name: "Simulation");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
