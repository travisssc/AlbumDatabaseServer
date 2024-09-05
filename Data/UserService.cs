using AlbumDatabaseServer.Components;
using AlbumDatabaseServer.Migrations;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace AlbumDatabaseServer.Data
{
    public class UserService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public UserService(IDbContextFactory<ApplicationDbContext> dbContextFactory, UserManager<IdentityUser> userManager)
        {
            _dbContextFactory = dbContextFactory;
        }
		private ApplicationDbContext CreateContext() => _dbContextFactory.CreateDbContext();
        // retrieve all user activity sorted by newest
		public async Task<List<UserActivity>> GetUserActivitiesAsync(string userName, int amount)
        {
            using var context = CreateContext();
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
                .Take(amount)
                .ToList();
            return allActivities;
        }
        public async Task<List<UserActivityDto>> GetNewActivityAsync(int amount)
        {
            using var context = CreateContext();
			var listenedActivities = await context.ListenedAlbums
				.Select(l => new ListenedActivity(l.AlbumId, l.UserName, l.DateListened))
				.ToListAsync();
			var favoriteActivities = await context.FavoriteAlbums
				.Select(f => new FavoriteActivity(f.AlbumId, f.UserName, f.DateFavorited))
				.ToListAsync();
			var queueActivities = await context.AccountQueue
				.Select(q => new QueueActivity(q.AlbumId, q.UserName, q.DateAdded))
				.ToListAsync();
			var ratingActivities = await context.AlbumRatings
				.Select(r => new RatingActivity(r.AlbumId, r.UserName, r.DateRated, r.Rating))
				.ToListAsync();
			var reviewActivities = await context.AlbumReviews
				.Select(r => new ReviewActivity(r.AlbumId, r.UserName, r.DateReviewed))
				.ToListAsync();
			var allActivities = listenedActivities.Cast<UserActivity>()
				.Concat(favoriteActivities)
				.Concat(queueActivities)
				.Concat(ratingActivities)
				.Concat(reviewActivities)
				.OrderByDescending(a => a.Date)
				.Take(amount)
				.ToList();
			var activityDtos = allActivities.Select(a => new UserActivityDto
			{
				AlbumId = a.AlbumId,
				UserName = a.UserName,
				ActivityType = a.ActivityType,
				Date = a.Date,
				AlbumName = a.AlbumName,
			}).ToList();
			return activityDtos;
		}
        // LISTEN FUNCTIONS
        public async Task<bool> IsAlbumListenedAsync(int albumId, string userName)
        {
			using var context = CreateContext();
			return await context.ListenedAlbums
                .AnyAsync(l => l.AlbumId == albumId && l.UserName == userName);
        }

        public async Task ToggleListenAsync(int albumId, string userName)
        {
			using var context = CreateContext();
			var listenedAlbum = await context.ListenedAlbums
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            if (listenedAlbum != null)
            {
                context.ListenedAlbums.Remove(listenedAlbum);
            }
            else
            {
				context.ListenedAlbums.Add(new ListenedAlbums
                {
                    AlbumId = albumId,
                    UserName = userName,
                    DateListened = DateTime.UtcNow
                });
            }
            await context.SaveChangesAsync();
        }
		public async Task<List<ListenedAlbums>> GetListenedAlbumsAsync(string userName)
		{
			using var context = CreateContext();
			return await context.ListenedAlbums
				.Where(f => f.UserName == userName)
				.OrderByDescending(f => f.DateListened)
				.ToListAsync();
		}
        public async Task<int> CountListenedAlbumsAsync(string userName)
        {
            using var context = CreateContext();
			return await context.ListenedAlbums
				.Where(l => l.UserName == userName)
				.CountAsync();
        }
        public async Task<DateTime> GetDateListenedAsync(string userName, int albumId)
		{
			using var context = CreateContext();
			var listenedAlbum = await context.ListenedAlbums
				.FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
			return listenedAlbum?.DateListened ?? DateTime.MinValue;
		}
		// FAVORITE FUNCTIONS
		public async Task<bool> IsAlbumFavoritedAsync(int albumId, string userName)
        {
			using var context = CreateContext();
			return await context.FavoriteAlbums
                .AnyAsync(l => l.AlbumId == albumId && l.UserName == userName);
        }

        public async Task ToggleFavoriteAsync(int albumId, string userName)
        {
			using var context = CreateContext();
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
			using var context = CreateContext();
			return await context.FavoriteAlbums
				.Where(f => f.UserName == userName)
				.OrderByDescending(f => f.DateFavorited)
				.ToListAsync();
		}
        public async Task<DateTime> GetDateFavoritedAsync(string userName, int albumId)
        {
            using var context = CreateContext();
			var favoritedAlbum = await context.FavoriteAlbums
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            return favoritedAlbum?.DateFavorited ?? DateTime.MinValue;
        }
        // QUEUE FUNCTIONS

        public async Task<bool> IsAlbumQueuedAsync(int albumId, string userName)
        {
			using var context = CreateContext();
			return await context.AccountQueue
                .AnyAsync(l => l.AlbumId == albumId && l.UserName == userName);
        }

        public async Task ToggleQueueAsync(int albumId, string userName)
        {
			using var context = CreateContext();
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
			using var context = CreateContext();
			return await context.AccountQueue
				.Where(q => q.UserName == userName)
				.CountAsync();
		}
		public async Task<List<AccountQueue>> GetQueuedAlbumsAsync(string userName)
		{
			using var context = CreateContext();
			return await context.AccountQueue
				.Where(f => f.UserName == userName)
				.OrderByDescending(f => f.DateAdded)
				.ToListAsync();
		}
        public async Task<DateTime> GetDateQueuedAsync(string userName, int albumId)
        {
            using var context = CreateContext();
			var queuedAlbum = await context.AccountQueue
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            return queuedAlbum?.DateAdded ?? DateTime.MinValue;
        }
        // RATING FUNCTIONS
        public async Task<AlbumRating> GetRatingAsync(int albumId, string userName)
        {
			using var context = CreateContext();
			return await context.AlbumRatings
                .FirstOrDefaultAsync(r => r.AlbumId == albumId && r.UserName == userName);
        }
        public async Task<int>  GetRatingIntAsync(int albumId, string userName)
        {
            using var context = CreateContext();
			return await context.AlbumRatings
				.Where(r => r.AlbumId == albumId && r.UserName == userName)
				.Select(r => r.Rating)
				.SingleOrDefaultAsync();
        }
        public async Task SubmitAlbumRatingAsync(int albumId, string userName, int rating)
        {
			using var context = CreateContext();
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
			using var context = CreateContext();
			var albumRating = await GetRatingAsync(albumId, userName);
            if (albumRating != null)
            {
                context.AlbumRatings.Remove(albumRating);
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateRatingAsync(AlbumRating rating)
        {
			using var context = CreateContext();
			context.AlbumRatings.Update(rating);
            await context.SaveChangesAsync();
        }
		public async Task<int> GetRatingCountAsync(string userName)
		{
			using var context = CreateContext();
			return await context.AlbumRatings
				.Where(r => r.UserName == userName)
				.CountAsync();
		}
        public async Task<List<double>> GetRatingDistributionAsync(string userName)
        {
            using var context = CreateContext();
			return await context.AlbumRatings
                .Where(r => r.UserName == userName)
                .Select(r => (double)r.Rating)
                .ToListAsync();
        }
		// REVIEW FUNCTIONS
		public async Task<AlbumReview> GetReviewAsync(int albumId, string userName)
        {
			using var context = CreateContext();
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
			using var context = CreateContext();
			var existingReview = await context.AlbumReviews
                .FirstOrDefaultAsync(r => r.AlbumId == albumId && r.UserName == userName);
            if (existingReview != null && !string.IsNullOrEmpty(existingReview.ReviewText))
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
            using var context = CreateContext();
			var review = await GetReviewAsync(albumId, userName);
            if (review != null)
            {
                context.AlbumReviews.Remove(review);
                await context.SaveChangesAsync();
            }
        }
		public async Task<List<AlbumReview>> GetReviewedAlbumsAsync(string userName)
        {
            using var context = CreateContext();
			return await context.AlbumReviews
                .Where(r => r.UserName == userName)
				.OrderByDescending(r => r.DateReviewed)
				.ToListAsync();
        }
        public async Task<DateTime> GetDateReviewedAsync(string userName, int albumId)
        {
            using var context = CreateContext();
			var reviewedAlbum = await context.AlbumReviews
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            return reviewedAlbum?.DateReviewed ?? DateTime.MinValue;
        }
        public async Task<AlbumReview> GetLatestReviewAsync(int albumId)
        {
            using var context = CreateContext();
			var latestReview = await context.AlbumReviews
                .Where(r => r.AlbumId == albumId)
                .OrderByDescending(r => r.DateReviewed)
                .FirstOrDefaultAsync();
            return latestReview;
        }
		public async Task<List<AlbumReview>> GetRecentReviewsAsync(int amount)
        {
            using var context = CreateContext();
			return await context.AlbumReviews
				.OrderByDescending(r => r.DateReviewed)
				.Take(amount)
				.ToListAsync();
		}
		// LIST FUNCTIONS
		public async Task<List<AlbumLists>> GetListsAsync(string userName)
		{
			using var context = CreateContext();
			return await context.Lists
                .Where(l => l.UserId == userName)
                .Include(l => l.ListAlbums)
				.OrderByDescending(l => l.DateUpdated)
				.ToListAsync();
		}
        public async Task<DateTime> GetDateListCreatedAsync(string userName, int listId)
        {
            using var context = CreateContext();
			var list = await context.Lists
				.FirstOrDefaultAsync(l => l.ListId == listId && l.UserId == userName);
            return list?.DateCreated ?? DateTime.MinValue;
        }
        public async Task<DateTime> GetDateListUpdatedAsync(string userName, int listId)
		{
			using var context = CreateContext();
			var list = await context.Lists
				.FirstOrDefaultAsync(l => l.ListId == listId && l.UserId == userName);
			return list?.DateUpdated ?? DateTime.MinValue;
		}
        public async Task<AlbumLists> GetListAsync(int listId, string userName)
        {
            using var context = CreateContext();
			return await context.Lists
                .Include(l => l.ListAlbums)
                .FirstOrDefaultAsync(l => l.ListId == listId && l.UserId == userName);
        }
        public async Task UpdateListDescription(int listId, string description)
        {
            using var context = CreateContext();
			var list = await context.Lists
                .FirstOrDefaultAsync(l => l.ListId == listId);
            if (list != null)
            {
                list.Description = description;
                list.DateUpdated = DateTime.UtcNow;
                context.Lists.Update(list);
                await context.SaveChangesAsync();
            }
        }
        // PROFILE PIC FUNCTIONS
        public async Task<string> GetAccountPicAsync(string userName)
		{
			using var context = CreateContext();
			var user = await context.AccountPictures
                .FirstOrDefaultAsync(p => p.UserName == userName);
			return user?.PicturePath ?? string.Empty;
		}
		public async Task<string> CreateFilePath(IBrowserFile image, string wwwrootPath, string folder)
		{
			var absPath = Path.Combine(wwwrootPath, folder);
			if (!Directory.Exists(absPath))
				Directory.CreateDirectory(absPath);
			var fileName = Path.GetRandomFileName() + Path.GetExtension(image.Name);
			var filePath = Path.Combine(absPath, fileName);

			await using var fs = new FileStream(filePath, FileMode.Create); // upload the file to the ABSOLUTE path
			await image.OpenReadStream().CopyToAsync(fs);
			return Path.Combine(folder, fileName); // return cover's RELATIVE path for the database entry
		}
		public async Task SubmitAccountPicAsync(string userName, IBrowserFile file, string wwwrootPath)
        {
			using var context = CreateContext();
			if (file == null || file.Size == 0)
            {
                throw new ArgumentException("No file provided");
            }
            const long maxFileSize = 2 * 1024 * 1024; // 2MB
            if (file.Size > maxFileSize)
            {
				throw new ArgumentException("Maximum file size is 2MB");
			}
			var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.Name).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !Array.Exists(allowedExtensions, ext => ext == extension))
                throw new ArgumentException("Invalid file type. Only .jpg, .jpeg, and .png are allowed.");
            var filePath = await CreateFilePath(file, wwwrootPath, "profile-pictures");
            var existingPic = await context.AccountPictures
				.FirstOrDefaultAsync(p => p.UserName == userName);
            if (existingPic != null)
			{
                var oldFilePath = Path.Combine(wwwrootPath, existingPic.PicturePath);
                if (File.Exists(oldFilePath))
				{
					File.Delete(oldFilePath);
				}
				existingPic.PicturePath = filePath;
				context.AccountPictures.Update(existingPic);
			}
			else
            {
				await context.AccountPictures.AddAsync(new AccountPicture
				{
					UserName = userName,
					PicturePath = filePath
				});
			}
			await context.SaveChangesAsync();
		}
		public async Task DeleteAccountPicAsync(string userName)
        {
			using var context = CreateContext();
			var profilePicture = await context.AccountPictures
                .FirstOrDefaultAsync(p => p.UserName == userName);
            if (profilePicture != null)
            {
                context.AccountPictures.Remove(profilePicture);
				await context.SaveChangesAsync();
            }
        }
        // ACTIVITY FUNCTIONS
        public async Task<List<AlbumTrendDto>> GetTrendingAlbumsAsync(int amt)
        {
            using var context = CreateContext();
			var activities = await (from r in context.AlbumRatings
									join f in context.FavoriteAlbums on r.AlbumId equals f.AlbumId into fav
									from f in fav.DefaultIfEmpty()
									join l in context.ListenedAlbums on r.AlbumId equals l.AlbumId into listened
									from l in listened.DefaultIfEmpty()
									join q in context.AccountQueue on r.AlbumId equals q.AlbumId into queue
									from q in queue.DefaultIfEmpty()
									join rev in context.AlbumReviews on r.AlbumId equals rev.AlbumId into reviews
									from rev in reviews.DefaultIfEmpty()
									group new { r, f, l, q, rev } by new { r.AlbumId, r.Album.Name, r.Album.Artist.ArtistName } into g
									select new AlbumTrendDto
									{
										AlbumId = g.Key.AlbumId,
										AlbumName = g.Key.Name,
										ArtistName = g.Key.ArtistName,
										ActivityCount = g.Count()
									})
							.OrderByDescending(a => a.ActivityCount)
							.Take(amt)
							.ToListAsync();

			return activities;
		}
	}
}