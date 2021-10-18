using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class AddLoanTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TasksToDo",
                table: "LoanApplications");

            migrationBuilder.CreateTable(
                name: "ClientTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Done = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientTask_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoanTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Done = table.Column<bool>(type: "INTEGER", nullable: false),
                    LoanApplicationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanTasks_LoanApplications_LoanApplicationId",
                        column: x => x.LoanApplicationId,
                        principalTable: "LoanApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientTask_ClientId",
                table: "ClientTask",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanTasks_LoanApplicationId",
                table: "LoanTasks",
                column: "LoanApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientTask");

            migrationBuilder.DropTable(
                name: "LoanTasks");

            migrationBuilder.AddColumn<string>(
                name: "TasksToDo",
                table: "LoanApplications",
                type: "TEXT",
                nullable: true);
        }
    }
}
