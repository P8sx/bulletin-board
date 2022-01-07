using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class GroupRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Roles_RoleId",
                table: "GroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_GroupUsers_RoleId",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "GroupUsers");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "GroupUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "GroupUsers");

            migrationBuilder.AddColumn<ulong>(
                name: "RoleId",
                table: "GroupUsers",
                type: "bigint unsigned",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_RoleId",
                table: "GroupUsers",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Roles_RoleId",
                table: "GroupUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
