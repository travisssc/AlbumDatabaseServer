namespace AlbumDatabaseServer.Data
{
	public class ListenedActivity : UserActivity
	{
		public ListenedActivity(int albumId, string userName, DateTime dateListened)
		{
			AlbumId = albumId;
			UserName = userName;
			Date = dateListened;
			ActivityType = "Listened";
		}
	}
}
