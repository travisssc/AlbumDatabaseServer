using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlbumDatabaseServer.Migrations
{
    /// <inheritdoc />
    public partial class nomorelistalbums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListAlbum_Albums_AlbumId",
                table: "ListAlbum");

            migrationBuilder.DropForeignKey(
                name: "FK_ListAlbum_Lists_ListId",
                table: "ListAlbum");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListAlbum",
                table: "ListAlbum");

            migrationBuilder.RenameTable(
                name: "ListAlbum",
                newName: "ListAlbums");

            migrationBuilder.RenameIndex(
                name: "IX_ListAlbum_ListId",
                table: "ListAlbums",
                newName: "IX_ListAlbums_ListId");

            migrationBuilder.RenameIndex(
                name: "IX_ListAlbum_AlbumId",
                table: "ListAlbums",
                newName: "IX_ListAlbums_AlbumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListAlbums",
                table: "ListAlbums",
                column: "ListAlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListAlbums_Albums_AlbumId",
                table: "ListAlbums",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "AlbumId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListAlbums_Lists_ListId",
                table: "ListAlbums",
                column: "ListId",
                principalTable: "Lists",
                principalColumn: "ListId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListAlbums_Albums_AlbumId",
                table: "ListAlbums");

            migrationBuilder.DropForeignKey(
                name: "FK_ListAlbums_Lists_ListId",
                table: "ListAlbums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListAlbums",
                table: "ListAlbums");

            migrationBuilder.RenameTable(
                name: "ListAlbums",
                newName: "ListAlbum");

            migrationBuilder.RenameIndex(
                name: "IX_ListAlbums_ListId",
                table: "ListAlbum",
                newName: "IX_ListAlbum_ListId");

            migrationBuilder.RenameIndex(
                name: "IX_ListAlbums_AlbumId",
                table: "ListAlbum",
                newName: "IX_ListAlbum_AlbumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListAlbum",
                table: "ListAlbum",
                column: "ListAlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListAlbum_Albums_AlbumId",
                table: "ListAlbum",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "AlbumId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListAlbum_Lists_ListId",
                table: "ListAlbum",
                column: "ListId",
                principalTable: "Lists",
                principalColumn: "ListId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
