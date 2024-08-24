namespace AlbumDatabaseServer.Data
{
	public class RatingActivity : UserActivity
	{
		public RatingActivity(int albumId, string userName, DateTime dateRated, int rating)
		{
			AlbumId = albumId;
			UserName = userName;
			Date = dateRated;
			ActivityType = "Rated";
			Rating = rating;
		}
		public int Rating { get; set; }
	}
}
