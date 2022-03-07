using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class BoardUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReadOnly",
                table: "Boards",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadOnly",
                table: "Boards");
        }
    }
}
