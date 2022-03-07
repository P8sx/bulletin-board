using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class ImageUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "Images",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "Images");
        }
    }
}
