namespace AlbumDatabaseServer.Data
{
	public class UserActivityDto
	{
		public int AlbumId { get; set; }
		public string AlbumName { get; set; }
		public string UserName { get; set; }
		public string ActivityType { get; set; }
		public DateTime Date { get; set; }
	}
}
