using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SplitwiseAPI.Migrations
{
    public partial class twefth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Activities");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Activities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Activities",
                nullable: true);
        }
    }
}
