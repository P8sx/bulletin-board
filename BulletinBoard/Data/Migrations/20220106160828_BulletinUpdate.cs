using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class BulletinUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Bulletins");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Bulletins");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "Bulletins",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "Bulletins",
                type: "float",
                nullable: true);
        }
    }
}
