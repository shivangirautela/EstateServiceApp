using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EstateServiceApp.Migrations
{
    public partial class PropertyDbContextM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    AvailableFrom = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    PostedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Properties");
        }
    }
}
