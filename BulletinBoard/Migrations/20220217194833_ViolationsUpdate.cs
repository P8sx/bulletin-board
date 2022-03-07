using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class ViolationsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "BoardId",
                table: "Violations",
                type: "bigint unsigned",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Pinned",
                table: "Bulletins",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Bulletins",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Violations_BoardId",
                table: "Violations",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Boards_BoardId",
                table: "Violations",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Boards_BoardId",
                table: "Violations");

            migrationBuilder.DropIndex(
                name: "IX_Violations_BoardId",
                table: "Violations");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Violations");

            migrationBuilder.AlterColumn<bool>(
                name: "Pinned",
                table: "Bulletins",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Bulletins",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");
        }
    }
}
