using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class PaidToLoanApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "LoanApplications",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_BankId",
                table: "LoanApplications",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_ClientId",
                table: "LoanApplications",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_Banks_BankId",
                table: "LoanApplications",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_Clients_ClientId",
                table: "LoanApplications",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_Banks_BankId",
                table: "LoanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_Clients_ClientId",
                table: "LoanApplications");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_BankId",
                table: "LoanApplications");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_ClientId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "LoanApplications");
        }
    }
}
