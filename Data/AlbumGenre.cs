namespace AlbumDatabaseServer.Data
{
    public class AlbumGenre
    {
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }
    }
}