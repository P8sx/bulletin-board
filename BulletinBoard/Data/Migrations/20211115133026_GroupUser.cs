using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class GroupUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Groups_GroupsId",
                table: "GroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Users_UsersId",
                table: "GroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Groups_GroupId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_GroupId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupUsers",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "UserRoles");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "GroupUsers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "GroupsId",
                table: "GroupUsers",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupUsers_UsersId",
                table: "GroupUsers",
                newName: "IX_GroupUsers_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "Image",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<ulong>(
                name: "Id",
                table: "GroupUsers",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "Joined",
                table: "GroupUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "RoleId",
                table: "GroupUsers",
                type: "bigint unsigned",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupUsers",
                table: "GroupUsers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupId",
                table: "GroupUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_RoleId",
                table: "GroupUsers",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Groups_GroupId",
                table: "GroupUsers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Roles_RoleId",
                table: "GroupUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Users_UserId",
                table: "GroupUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Groups_GroupId",
                table: "GroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Roles_RoleId",
                table: "GroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Users_UserId",
                table: "GroupUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupUsers",
                table: "GroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_GroupUsers_GroupId",
                table: "GroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_GroupUsers_RoleId",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "Joined",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "GroupUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "GroupUsers",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "GroupUsers",
                newName: "GroupsId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupUsers_UserId",
                table: "GroupUsers",
                newName: "IX_GroupUsers_UsersId");

            migrationBuilder.AddColumn<ulong>(
                name: "GroupId",
                table: "UserRoles",
                type: "bigint unsigned",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "Image",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupUsers",
                table: "GroupUsers",
                columns: new[] { "GroupsId", "UsersId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_GroupId",
                table: "UserRoles",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Groups_GroupsId",
                table: "GroupUsers",
                column: "GroupsId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Users_UsersId",
                table: "GroupUsers",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Groups_GroupId",
                table: "UserRoles",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
