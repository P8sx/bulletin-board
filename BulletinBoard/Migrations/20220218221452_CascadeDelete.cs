using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Images_ImageId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Bulletins_Boards_BoardId",
                table: "Bulletins");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Bulletins_BulletinId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Bulletins_BulletinId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ImageId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Boards_BoardId",
                table: "Violations");

            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Bulletins_BulletinId",
                table: "Violations");

            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Comments_CommentId",
                table: "Violations");

            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Users_UserId",
                table: "Violations");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Images_ImageId",
                table: "Boards",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bulletins_Boards_BoardId",
                table: "Bulletins",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Bulletins_BulletinId",
                table: "Comments",
                column: "BulletinId",
                principalTable: "Bulletins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Bulletins_BulletinId",
                table: "Images",
                column: "BulletinId",
                principalTable: "Bulletins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_ImageId",
                table: "Users",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Boards_BoardId",
                table: "Violations",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Bulletins_BulletinId",
                table: "Violations",
                column: "BulletinId",
                principalTable: "Bulletins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Comments_CommentId",
                table: "Violations",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Users_UserId",
                table: "Violations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Images_ImageId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Bulletins_Boards_BoardId",
                table: "Bulletins");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Bulletins_BulletinId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Bulletins_BulletinId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ImageId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Boards_BoardId",
                table: "Violations");

            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Bulletins_BulletinId",
                table: "Violations");

            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Comments_CommentId",
                table: "Violations");

            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Users_UserId",
                table: "Violations");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Images_ImageId",
                table: "Boards",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bulletins_Boards_BoardId",
                table: "Bulletins",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Bulletins_BulletinId",
                table: "Comments",
                column: "BulletinId",
                principalTable: "Bulletins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Bulletins_BulletinId",
                table: "Images",
                column: "BulletinId",
                principalTable: "Bulletins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_ImageId",
                table: "Users",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Boards_BoardId",
                table: "Violations",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Bulletins_BulletinId",
                table: "Violations",
                column: "BulletinId",
                principalTable: "Bulletins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Comments_CommentId",
                table: "Violations",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Users_UserId",
                table: "Violations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
