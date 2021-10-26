using Microsoft.EntityFrameworkCore.Migrations;

namespace EstateServiceApp.Migrations
{
    public partial class useractivity3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "UserActivites");

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "UserActivites",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "UserActivites");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "UserActivites",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
