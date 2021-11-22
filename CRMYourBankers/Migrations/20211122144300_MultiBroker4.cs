using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class MultiBroker4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_MultiBroker_MultiBrokerId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_MultiBrokerId",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MultiBroker",
                table: "MultiBroker");

            migrationBuilder.DropColumn(
                name: "MultiBrokerId",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "MultiBroker",
                newName: "MultiBrokers");

            migrationBuilder.AddColumn<int>(
                name: "MultiBrokerId",
                table: "LoanApplications",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MultiBrokers",
                table: "MultiBrokers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_MultiBrokerId",
                table: "LoanApplications",
                column: "MultiBrokerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_MultiBrokers_MultiBrokerId",
                table: "LoanApplications",
                column: "MultiBrokerId",
                principalTable: "MultiBrokers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_MultiBrokers_MultiBrokerId",
                table: "LoanApplications");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_MultiBrokerId",
                table: "LoanApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MultiBrokers",
                table: "MultiBrokers");

            migrationBuilder.DropColumn(
                name: "MultiBrokerId",
                table: "LoanApplications");

            migrationBuilder.RenameTable(
                name: "MultiBrokers",
                newName: "MultiBroker");

            migrationBuilder.AddColumn<int>(
                name: "MultiBrokerId",
                table: "Clients",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MultiBroker",
                table: "MultiBroker",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_MultiBrokerId",
                table: "Clients",
                column: "MultiBrokerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_MultiBroker_MultiBrokerId",
                table: "Clients",
                column: "MultiBrokerId",
                principalTable: "MultiBroker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
