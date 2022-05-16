using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class BIKProposalNoteAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BIKNote",
                table: "Clients",
                newName: "BIKProposalNote6");

            migrationBuilder.AddColumn<string>(
                name: "BIKProposalNote1",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BIKProposalNote2",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BIKProposalNote3",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BIKProposalNote4",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BIKProposalNote5",
                table: "Clients",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BIKProposalNote1",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "BIKProposalNote2",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "BIKProposalNote3",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "BIKProposalNote4",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "BIKProposalNote5",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "BIKProposalNote6",
                table: "Clients",
                newName: "BIKNote");
        }
    }
}
