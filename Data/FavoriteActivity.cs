namespace AlbumDatabaseServer.Data
{
	public class FavoriteActivity : UserActivity
	{
		public FavoriteActivity(int albumId, string userName, DateTime dateFavorited)
		{
			AlbumId = albumId;
			UserName = userName;
			Date = dateFavorited;
			ActivityType = "Favorited";
		}
	}
}
