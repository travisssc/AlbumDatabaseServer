using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlbumDatabaseServer.Migrations
{
    /// <inheritdoc />
    public partial class artistpictures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArtistPicture",
                table: "Artists",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtistPicture",
                table: "Artists");
        }
    }
}
