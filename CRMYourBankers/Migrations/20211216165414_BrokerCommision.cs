using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class BrokerCommision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrokerCommission",
                table: "LoanApplications",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrokerCommission",
                table: "LoanApplications");
        }
    }
}
