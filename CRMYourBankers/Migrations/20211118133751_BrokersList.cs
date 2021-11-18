using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class BrokersList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrokerId",
                table: "Clients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Brokers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_BrokerId",
                table: "Clients",
                column: "BrokerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Brokers_BrokerId",
                table: "Clients",
                column: "BrokerId",
                principalTable: "Brokers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Brokers_BrokerId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "Brokers");

            migrationBuilder.DropIndex(
                name: "IX_Clients_BrokerId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "BrokerId",
                table: "Clients");
        }
    }
}
