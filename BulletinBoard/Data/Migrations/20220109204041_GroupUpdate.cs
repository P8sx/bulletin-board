using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class GroupUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Public",
                table: "Groups");

            migrationBuilder.AddColumn<bool>(
                name: "AcceptAnyone",
                table: "Groups",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PublicListed",
                table: "Groups",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptAnyone",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "PublicListed",
                table: "Groups");

            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Groups",
                type: "tinyint(1)",
                nullable: true);
        }
    }
}
