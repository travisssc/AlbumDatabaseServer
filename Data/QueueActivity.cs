namespace AlbumDatabaseServer.Data
{
	public class QueueActivity : UserActivity
	{
		public QueueActivity(int albumId, string userName, DateTime dateQueued)
		{
			AlbumId = albumId;
			UserName = userName;
			Date = dateQueued;
			ActivityType = "Queued";
		}
	}
}
