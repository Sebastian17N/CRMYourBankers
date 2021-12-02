using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class expandedPrimaryKeyForBankClientPersonalLoans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BankClientPersonalLoans",
                table: "BankClientPersonalLoans");

            migrationBuilder.DropColumn(
                name: "LoanIndex",
                table: "BankClientPersonalLoans");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankClientPersonalLoans",
                table: "BankClientPersonalLoans",
                columns: new[] { "BankId", "ClientId", "BIKType" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BankClientPersonalLoans",
                table: "BankClientPersonalLoans");

            migrationBuilder.AddColumn<int>(
                name: "LoanIndex",
                table: "BankClientPersonalLoans",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankClientPersonalLoans",
                table: "BankClientPersonalLoans",
                columns: new[] { "BankId", "ClientId" });
        }
    }
}
