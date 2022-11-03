using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMYourBankers.Migrations
{
    public partial class TaskAddedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TaskAddedDate",
                table: "ClientTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskAddedDate",
                table: "ClientTasks");
        }
    }
}
