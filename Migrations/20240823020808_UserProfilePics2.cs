using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlbumDatabaseServer.Migrations
{
    /// <inheritdoc />
    public partial class UserProfilePics2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserProfilePictures_UserId",
                table: "UserProfilePictures",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfilePictures_AspNetUsers_UserId",
                table: "UserProfilePictures",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfilePictures_AspNetUsers_UserId",
                table: "UserProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_UserProfilePictures_UserId",
                table: "UserProfilePictures");
        }
    }
}
