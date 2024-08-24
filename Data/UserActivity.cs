namespace AlbumDatabaseServer.Data
{
	public abstract class UserActivity
	{
		public DateTime Date { get; set; }
		public string UserName { get; set; }
		public string ActivityType { get; set; }
		public int AlbumId { get; set; }
		public string AlbumName { get; set; }
	}
}
