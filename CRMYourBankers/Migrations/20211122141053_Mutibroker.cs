using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class Mutibroker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MultiBrokerId",
                table: "LoanApplications",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MultiBrokerId",
                table: "Clients",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MultiBrokerId",
                table: "BankClientPersonalLoans",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MultiBroker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiBroker", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_MultiBrokerId",
                table: "LoanApplications",
                column: "MultiBrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_MultiBrokerId",
                table: "Clients",
                column: "MultiBrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_BankClientPersonalLoans_MultiBrokerId",
                table: "BankClientPersonalLoans",
                column: "MultiBrokerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankClientPersonalLoans_MultiBroker_MultiBrokerId",
                table: "BankClientPersonalLoans",
                column: "MultiBrokerId",
                principalTable: "MultiBroker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_MultiBroker_MultiBrokerId",
                table: "Clients",
                column: "MultiBrokerId",
                principalTable: "MultiBroker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_MultiBroker_MultiBrokerId",
                table: "LoanApplications",
                column: "MultiBrokerId",
                principalTable: "MultiBroker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankClientPersonalLoans_MultiBroker_MultiBrokerId",
                table: "BankClientPersonalLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_MultiBroker_MultiBrokerId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_MultiBroker_MultiBrokerId",
                table: "LoanApplications");

            migrationBuilder.DropTable(
                name: "MultiBroker");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_MultiBrokerId",
                table: "LoanApplications");

            migrationBuilder.DropIndex(
                name: "IX_Clients_MultiBrokerId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_BankClientPersonalLoans_MultiBrokerId",
                table: "BankClientPersonalLoans");

            migrationBuilder.DropColumn(
                name: "MultiBrokerId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "MultiBrokerId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "MultiBrokerId",
                table: "BankClientPersonalLoans");
        }
    }
}
