using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlbumDatabaseServer.Migrations
{
    /// <inheritdoc />
    public partial class updateisnow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SongCount",
                table: "Albums");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SongCount",
                table: "Albums",
                type: "integer",
                nullable: true);
        }
    }
}
