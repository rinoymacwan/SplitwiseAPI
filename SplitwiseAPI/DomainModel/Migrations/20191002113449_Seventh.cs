using Microsoft.EntityFrameworkCore.Migrations;

namespace SplitwiseAPI.Migrations
{
    public partial class Seventh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Settlements",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Settlements",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
