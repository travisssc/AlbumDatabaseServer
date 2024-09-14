namespace AlbumDatabaseServer.Data
{
    public class CarouselState
    {
        public string CarouselId { get; set; } = Guid.NewGuid().ToString();
        public List<Album> Albums { get; set; }
        public string Size { get; set; } = "large";
	}
}
