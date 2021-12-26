using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class GroupUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Groups",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ImageId",
                table: "Groups",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Image_ImageId",
                table: "Groups",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Image_ImageId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ImageId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Groups");
        }
    }
}
