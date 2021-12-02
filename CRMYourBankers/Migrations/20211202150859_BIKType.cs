using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class BIKType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BIKType",
                table: "BankClientPersonalLoans",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoanIndex",
                table: "BankClientPersonalLoans",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BIKType",
                table: "BankClientPersonalLoans");

            migrationBuilder.DropColumn(
                name: "LoanIndex",
                table: "BankClientPersonalLoans");
        }
    }
}
