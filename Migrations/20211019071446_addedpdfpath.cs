using Microsoft.EntityFrameworkCore.Migrations;

namespace EstateServiceApp.Migrations
{
    public partial class addedpdfpath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfPath",
                table: "Properties",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfPath",
                table: "Properties");
        }
    }
}
