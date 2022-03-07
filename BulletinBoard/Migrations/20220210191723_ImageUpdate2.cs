using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class ImageUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileExtension",
                table: "Images",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileExtension",
                table: "Images",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
