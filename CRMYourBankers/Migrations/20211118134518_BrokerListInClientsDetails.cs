using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class BrokerListInClientsDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Brokers_BrokerId",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brokers",
                table: "Brokers");

            migrationBuilder.RenameTable(
                name: "Brokers",
                newName: "Broker");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Broker",
                table: "Broker",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Broker_BrokerId",
                table: "Clients",
                column: "BrokerId",
                principalTable: "Broker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Broker_BrokerId",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Broker",
                table: "Broker");

            migrationBuilder.RenameTable(
                name: "Broker",
                newName: "Brokers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brokers",
                table: "Brokers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Brokers_BrokerId",
                table: "Clients",
                column: "BrokerId",
                principalTable: "Brokers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
