namespace AlbumDatabaseServer.Data
{
	public class ReviewActivity : UserActivity
	{
		public ReviewActivity(int albumId, string userName, DateTime dateReviewed)
		{
			AlbumId = albumId;
			UserName = userName;
			Date = dateReviewed;
			ActivityType = "Reviewed";
		}
	}
}
