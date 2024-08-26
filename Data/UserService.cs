using AlbumDatabaseServer.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;

namespace AlbumDatabaseServer.Data
{
    public class UserService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public UserService(IDbContextFactory<ApplicationDbContext> dbContextFactory, UserManager<IdentityUser> userManager)
        {
            _dbContextFactory = dbContextFactory;
        }

        // retrieve all user activity sorted by newest
        public async Task<List<UserActivity>> GetUserActivitiesAsync(string userName)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var listenedActivities = await context.ListenedAlbums
                .Where(l => l.UserName == userName)
				.Select(l => new ListenedActivity(l.AlbumId, l.UserName, l.DateListened))
				.ToListAsync();
            var favoriteActivities = await context.FavoriteAlbums
				.Where(f => f.UserName == userName)
                .Select(f => new FavoriteActivity(f.AlbumId, f.UserName, f.DateFavorited))
                .ToListAsync();
            var queueActivities = await context.AccountQueue
                .Where(q => q.UserName == userName)
                .Select(q => new QueueActivity(q.AlbumId, q.UserName, q.DateAdded))
                .ToListAsync();
            var ratingActivities = await context.AlbumRatings
				.Where(r => r.UserName == userName)
				.Select(r => new RatingActivity(r.AlbumId, r.UserName, r.DateRated, r.Rating))
				.ToListAsync();
            var reviewActivities = await context.AlbumReviews
                .Where(r => r.UserName == userName)
                .Select(r => new ReviewActivity(r.AlbumId, r.UserName, r.DateReviewed))
                .ToListAsync();
            var allActivities = listenedActivities.Cast<UserActivity>()
                .Concat(favoriteActivities)
                .Concat(queueActivities)
                .Concat(ratingActivities)
                .Concat(reviewActivities)
                .OrderByDescending(a => a.Date)
                .Take(5)
                .ToList();
            return allActivities;
        }
        // LISTEN FUNCTIONS
        public async Task<bool> IsAlbumListenedAsync(int albumId, string userName)
        {
			using var context = _dbContextFactory.CreateDbContext();
			return await context.ListenedAlbums
                .AnyAsync(l => l.AlbumId == albumId && l.UserName == userName);
        }

        public async Task ToggleListenAsync(int albumId, string userName)
        {
			using var context = _dbContextFactory.CreateDbContext();
			var listenedAlbum = await context.ListenedAlbums
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            if (listenedAlbum != null)
            {
                context.ListenedAlbums.Remove(listenedAlbum);
            }
            else
            {
                listenedAlbum = new ListenedAlbums
                {
                    AlbumId = albumId,
                    UserName = userName,
                    DateListened = DateTime.UtcNow
                };
                context.ListenedAlbums.Add(listenedAlbum);
            }
            await context.SaveChangesAsync();
        }
		public async Task<List<ListenedAlbums>> GetListenedAlbumsAsync(string userName)
		{
			using var context = _dbContextFactory.CreateDbContext();
			return await context.ListenedAlbums
				.Where(f => f.UserName == userName)
				.OrderByDescending(f => f.DateListened)
				.ToListAsync();
		}
        public async Task<int> CountListenedAlbumsAsync(string userName)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.ListenedAlbums
				.Where(l => l.UserName == userName)
				.CountAsync();
        }
        public async Task<DateTime> GetDateListenedAsync(string userName, int albumId)
		{
			using var context = _dbContextFactory.CreateDbContext();
			var listenedAlbum = await context.ListenedAlbums
				.FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
			return listenedAlbum?.DateListened ?? DateTime.MinValue;
		}
		// FAVORITE FUNCTIONS
		public async Task<bool> IsAlbumFavoritedAsync(int albumId, string userName)
        {
			using var context = _dbContextFactory.CreateDbContext();
			return await context.FavoriteAlbums
                .AnyAsync(l => l.AlbumId == albumId && l.UserName == userName);
        }

        public async Task ToggleFavoriteAsync(int albumId, string userName)
        {
			using var context = _dbContextFactory.CreateDbContext();
			var favoritedAlbum = await context.FavoriteAlbums
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            if (favoritedAlbum != null)
            {
                context.FavoriteAlbums.Remove(favoritedAlbum);
            }
            else
            {
                favoritedAlbum = new FavoriteAlbums
                {
                    AlbumId = albumId,
                    UserName = userName,
                    DateFavorited = DateTime.UtcNow
                };
                context.FavoriteAlbums.Add(favoritedAlbum);
            }
            await context.SaveChangesAsync();
        }
        public async Task<List<FavoriteAlbums>> GetFavoriteAlbumsAsync(string userName)
		{
			using var context = _dbContextFactory.CreateDbContext();
			return await context.FavoriteAlbums
				.Where(f => f.UserName == userName)
				.OrderByDescending(f => f.DateFavorited)
				.ToListAsync();
		}
        public async Task<DateTime> GetDateFavoritedAsync(string userName, int albumId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var favoritedAlbum = await context.FavoriteAlbums
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            return favoritedAlbum?.DateFavorited ?? DateTime.MinValue;
        }
        // QUEUE FUNCTIONS

        public async Task<bool> IsAlbumQueuedAsync(int albumId, string userName)
        {
			using var context = _dbContextFactory.CreateDbContext();
			return await context.AccountQueue
                .AnyAsync(l => l.AlbumId == albumId && l.UserName == userName);
        }

        public async Task ToggleQueueAsync(int albumId, string userName)
        {
			using var context = _dbContextFactory.CreateDbContext();
			var queuedAlbum = await context.AccountQueue
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            if (queuedAlbum != null)
            {
                context.AccountQueue.Remove(queuedAlbum);
            }
            else
            {
                queuedAlbum = new AccountQueue
                {
                    AlbumId = albumId,
                    UserName = userName,
                    DateAdded = DateTime.UtcNow
                };
                context.AccountQueue.Add(queuedAlbum);
            }
            await context.SaveChangesAsync();
        }
        public async Task<int> GetQueueCountAsync(string userName)
		{
			using var context = _dbContextFactory.CreateDbContext();
			return await context.AccountQueue
				.Where(q => q.UserName == userName)
				.CountAsync();
		}
		public async Task<List<AccountQueue>> GetQueuedAlbumsAsync(string userName)
		{
			using var context = _dbContextFactory.CreateDbContext();
			return await context.AccountQueue
				.Where(f => f.UserName == userName)
				.OrderByDescending(f => f.DateAdded)
				.ToListAsync();
		}
        public async Task<DateTime> GetDateQueuedAsync(string userName, int albumId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var queuedAlbum = await context.AccountQueue
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            return queuedAlbum?.DateAdded ?? DateTime.MinValue;
        }
        // RATING FUNCTIONS
        public async Task<AlbumRating> GetRatingAsync(int albumId, string userName)
        {
			using var context = _dbContextFactory.CreateDbContext();
            return await context.AlbumRatings
                .FirstOrDefaultAsync(r => r.AlbumId == albumId && r.UserName == userName);
        }
        public async Task<int>  GetRatingIntAsync(int albumId, string userName)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.AlbumRatings
				.Where(r => r.AlbumId == albumId && r.UserName == userName)
				.Select(r => r.Rating)
				.FirstOrDefaultAsync();
        }
        public async Task SubmitAlbumRatingAsync(int albumId, string userName, int rating)
        {
			using var context = _dbContextFactory.CreateDbContext();
			var albumRating = await context.AlbumRatings
                .FirstOrDefaultAsync(r => r.AlbumId == albumId && r.UserName == userName);
            if (albumRating != null)
            {
                albumRating.Rating = rating;
                albumRating.DateRated = DateTime.UtcNow;
                context.AlbumRatings.Update(albumRating);
            }
            else
            {
                albumRating = new AlbumRating
                {
                    AlbumId = albumId,
                    UserName = userName,
                    Rating = rating,
                    DateRated = DateTime.UtcNow
                };
                context.AlbumRatings.Add(albumRating);
            }
            await context.SaveChangesAsync();
        }
        public async Task RemoveRatingAsync(int albumId, string userName)
        {
			using var context = _dbContextFactory.CreateDbContext();
			var albumRating = await GetRatingAsync(albumId, userName);
            if (albumRating != null)
            {
                context.AlbumRatings.Remove(albumRating);
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateRatingAsync(AlbumRating rating)
        {
			using var context = _dbContextFactory.CreateDbContext();
			context.AlbumRatings.Update(rating);
            await context.SaveChangesAsync();
        }
		public async Task<int> GetRatingCountAsync(string userName)
		{
			using var context = _dbContextFactory.CreateDbContext();
			return await context.AlbumRatings
				.Where(r => r.UserName == userName)
				.CountAsync();
		}
        public async Task<List<double>> GetRatingDistributionAsync(string userName)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.AlbumRatings
                .Where(r => r.UserName == userName)
                .Select(r => (double)r.Rating)
                .ToListAsync();
        }
		// REVIEW FUNCTIONS
		public async Task<AlbumReview> GetReviewAsync(int albumId, string userName)
        {
			using var context = _dbContextFactory.CreateDbContext();
			return await context.AlbumReviews
                .FirstOrDefaultAsync(r => r.AlbumId == albumId && r.UserName == userName)
                ?? new AlbumReview
                {
                    AlbumId = albumId,
                    UserName = userName,
                    ReviewText = string.Empty,
                    DateReviewed = DateTime.UtcNow
                };
        }
        public async Task SubmitReviewAsync(int albumId, string userName, string reviewText)
        {
			using var context = _dbContextFactory.CreateDbContext();
            var existingReview = await context.AlbumReviews
                .FirstOrDefaultAsync(r => r.AlbumId == albumId && r.UserName == userName);
            if (!string.IsNullOrEmpty(existingReview.ReviewText))
            {
                existingReview.ReviewText = reviewText;
                existingReview.DateReviewed = DateTime.UtcNow;
                context.AlbumReviews.Update(existingReview);
            }
            else
            {
                var newReview = new AlbumReview
                {
                    AlbumId = albumId,
                    UserName = userName,
                    ReviewText = reviewText,
                    DateReviewed = DateTime.UtcNow
                };
                context.AlbumReviews.Add(newReview);
            }
            await context.SaveChangesAsync();
        }
        public async Task RemoveReviewAsync(int albumId, string userName)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var review = await GetReviewAsync(albumId, userName);
            if (review != null)
            {
                context.AlbumReviews.Remove(review);
                await context.SaveChangesAsync();
            }
        }
		public async Task<List<AlbumReview>> GetReviewedAlbumsAsync(string userName)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.AlbumReviews
                .Where(r => r.UserName == userName)
				.OrderByDescending(r => r.DateReviewed)
				.ToListAsync();
        }
        public async Task<DateTime> GetDateReviewedAsync(string userName, int albumId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var reviewedAlbum = await context.AlbumReviews
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            return reviewedAlbum?.DateReviewed ?? DateTime.MinValue;
        }

        // LIST FUNCTIONS
        public async Task<List<AlbumLists>> GetListsAsync(string userName)
		{
			using var context = _dbContextFactory.CreateDbContext();
			return await context.Lists
				.Where(l => l.UserId == userName)
				.OrderByDescending(l => l.DateUpdated)
				.ToListAsync();
		}
        public async Task<DateTime> GetDateListCreatedAsync(string userName, int listId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var list = await context.Lists
				.FirstOrDefaultAsync(l => l.ListId == listId && l.UserId == userName);
            return list?.DateCreated ?? DateTime.MinValue;
        }
        public async Task<DateTime> GetDateListUpdatedAsync(string userName, int listId)
		{
			using var context = _dbContextFactory.CreateDbContext();
			var list = await context.Lists
				.FirstOrDefaultAsync(l => l.ListId == listId && l.UserId == userName);
			return list?.DateUpdated ?? DateTime.MinValue;
		}
    }
}