using Microsoft.EntityFrameworkCore.Migrations;

namespace EstateServiceApp.Migrations
{
    public partial class useractivity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "UserActivites");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "UserActivites",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "UserActivites");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "UserActivites",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
