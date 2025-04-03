using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EatWellAssistant.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_userId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_userId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "RequiredChanged",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreateUserId",
                table: "Posts",
                column: "CreateUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_CreateUserId",
                table: "Posts",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_CreateUserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CreateUserId",
                table: "Posts");

            migrationBuilder.AddColumn<bool>(
                name: "RequiredChanged",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_userId",
                table: "Posts",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_userId",
                table: "Posts",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId");
        }
    }
}
