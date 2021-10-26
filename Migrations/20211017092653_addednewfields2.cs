using Microsoft.EntityFrameworkCore.Migrations;

namespace EstateServiceApp.Migrations
{
    public partial class addednewfields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BathRooms",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BedRooms",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Properties",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Parking",
                table: "Properties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PropertyArea",
                table: "Properties",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPhotos",
                table: "Properties",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BathRooms",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "BedRooms",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Parking",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PropertyArea",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "TotalPhotos",
                table: "Properties");
        }
    }
}
