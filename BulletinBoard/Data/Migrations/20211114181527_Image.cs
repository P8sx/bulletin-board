using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class Image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentFiles",
                table: "Bulletins");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    GroupId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    BulletinId = table.Column<ulong>(type: "bigint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Bulletins_BulletinId",
                        column: x => x.BulletinId,
                        principalTable: "Bulletins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Image_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Image_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Image_BulletinId",
                table: "Image",
                column: "BulletinId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_GroupId",
                table: "Image",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_UserId",
                table: "Image",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentFiles",
                table: "Bulletins",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
