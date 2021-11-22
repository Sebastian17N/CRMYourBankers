using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class Mutibroker2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankClientPersonalLoans_MultiBroker_MultiBrokerId",
                table: "BankClientPersonalLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_MultiBroker_MultiBrokerId",
                table: "LoanApplications");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_MultiBrokerId",
                table: "LoanApplications");

            migrationBuilder.DropIndex(
                name: "IX_BankClientPersonalLoans_MultiBrokerId",
                table: "BankClientPersonalLoans");

            migrationBuilder.DropColumn(
                name: "MultiBrokerId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "MultiBrokerId",
                table: "BankClientPersonalLoans");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MultiBrokerId",
                table: "LoanApplications",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MultiBrokerId",
                table: "BankClientPersonalLoans",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_MultiBrokerId",
                table: "LoanApplications",
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
                name: "FK_LoanApplications_MultiBroker_MultiBrokerId",
                table: "LoanApplications",
                column: "MultiBrokerId",
                principalTable: "MultiBroker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
