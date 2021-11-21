using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.Migrations
{
    public partial class VotesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownVotes",
                table: "Bulletins");

            migrationBuilder.DropColumn(
                name: "UpVotes",
                table: "Bulletins");

            migrationBuilder.CreateTable(
                name: "BulletinsVotes",
                columns: table => new
                {
                    UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    BulletinId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulletinsVotes", x => new { x.UserId, x.BulletinId });
                    table.ForeignKey(
                        name: "FK_BulletinsVotes_Bulletins_BulletinId",
                        column: x => x.BulletinId,
                        principalTable: "Bulletins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BulletinsVotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BulletinsVotes_BulletinId",
                table: "BulletinsVotes",
                column: "BulletinId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BulletinsVotes");

            migrationBuilder.AddColumn<uint>(
                name: "DownVotes",
                table: "Bulletins",
                type: "int unsigned",
                nullable: true);

            migrationBuilder.AddColumn<uint>(
                name: "UpVotes",
                table: "Bulletins",
                type: "int unsigned",
                nullable: true);
        }
    }
}
