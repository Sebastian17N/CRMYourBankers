using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class ChangeNameBankClientPersonalLoanToBIK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankClientPersonalLoans");

            migrationBuilder.CreateTable(
                name: "BankClientBIK",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    BankId = table.Column<int>(type: "INTEGER", nullable: false),
                    BIKType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankClientBIK", x => new { x.BankId, x.ClientId, x.BIKType });
                    table.ForeignKey(
                        name: "FK_BankClientBIK_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankClientBIK_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankClientBIK_ClientId",
                table: "BankClientBIK",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankClientBIK");

            migrationBuilder.CreateTable(
                name: "BankClientPersonalLoans",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    BIKType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankClientPersonalLoans", x => new { x.BankId, x.ClientId, x.BIKType });
                    table.ForeignKey(
                        name: "FK_BankClientPersonalLoans_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankClientPersonalLoans_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankClientPersonalLoans_ClientId",
                table: "BankClientPersonalLoans",
                column: "ClientId");
        }
    }
}
