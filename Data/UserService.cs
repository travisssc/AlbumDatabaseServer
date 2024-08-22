using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AlbumDatabaseServer.Data
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
        }

        // LISTEN FUNCTIONS
        public async Task<bool> IsAlbumListenedAsync(int albumId, string userName)
        {
            return await _context.ListenedAlbums
                .AnyAsync(l => l.AlbumId == albumId && l.UserName == userName);
        }

        public async Task ToggleListenAsync(int albumId, string userName)
        {
            var listenedAlbum = await _context.ListenedAlbums
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            if (listenedAlbum != null)
            {
                _context.ListenedAlbums.Remove(listenedAlbum);
            }
            else
            {
                listenedAlbum = new ListenedAlbums
                {
                    AlbumId = albumId,
                    UserName = userName,
                    DateListened = DateTime.UtcNow
                };
                _context.ListenedAlbums.Add(listenedAlbum);
            }
            await _context.SaveChangesAsync();
        }

        // FAVORITE FUNCTIONS
        public async Task<bool> IsAlbumFavoritedAsync(int albumId, string userName)
        {
            return await _context.FavoriteAlbums
                .AnyAsync(l => l.AlbumId == albumId && l.UserName == userName);
        }

        public async Task ToggleFavoriteAsync(int albumId, string userName)
        {
            var favoritedAlbum = await _context.FavoriteAlbums
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            if (favoritedAlbum != null)
            {
                _context.FavoriteAlbums.Remove(favoritedAlbum);
            }
            else
            {
                favoritedAlbum = new FavoriteAlbums
                {
                    AlbumId = albumId,
                    UserName = userName,
                    DateFavorited = DateTime.UtcNow
                };
                _context.FavoriteAlbums.Add(favoritedAlbum);
            }
            await _context.SaveChangesAsync();
        }

        // QUEUE FUNCTIONS

        public async Task<bool> IsAlbumQueuedAsync(int albumId, string userName)
        {
            return await _context.AccountQueue
                .AnyAsync(l => l.AlbumId == albumId && l.UserName == userName);
        }

        public async Task ToggleQueueAsync(int albumId, string userName)
        {
            var queuedAlbum = await _context.AccountQueue
                .FirstOrDefaultAsync(l => l.AlbumId == albumId && l.UserName == userName);
            if (queuedAlbum != null)
            {
                _context.AccountQueue.Remove(queuedAlbum);
            }
            else
            {
                queuedAlbum = new AccountQueue
                {
                    AlbumId = albumId,
                    UserName = userName,
                    DateAdded = DateTime.UtcNow
                };
                _context.AccountQueue.Add(queuedAlbum);
            }
            await _context.SaveChangesAsync();
        }

        // RATING FUNCTIONS
        public async Task<AlbumRating> GetRatingAsync(int albumId, string userName)
        {
            return await _context.AlbumRatings
                .FirstOrDefaultAsync(r => r.AlbumId == albumId && r.UserName == userName)
                ?? new AlbumRating
                {
                    AlbumId = albumId,
                    UserName = userName,
                    Rating = 0,
                    DateRated = DateTime.UtcNow
                };
        }
        public async Task SubmitAlbumRatingAsync(int albumId, string userName, int rating)
        {
            var albumRating = await GetRatingAsync(albumId, userName);
            if (albumRating.Rating != 0)
            {
                albumRating.Rating = rating;
                albumRating.DateRated = DateTime.UtcNow;
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
                _context.AlbumRatings.Add(albumRating);
            }
            await _context.SaveChangesAsync();
        }
        public async Task RemoveRatingAsync(int albumId, string userName)
        {
            var albumRating = await GetRatingAsync(albumId, userName);
            if (albumRating != null)
            {
                _context.AlbumRatings.Remove(albumRating);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateRatingAsync(AlbumRating rating)
        {
            _context.AlbumRatings.Update(rating);
            await _context.SaveChangesAsync();
        }

        // REVIEW FUNCTIONS
        public async Task<AlbumReview> GetReviewAsync(int albumId, string userName)
        {
            return await _context.AlbumReviews
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
            var existingReview = await GetReviewAsync(albumId, userName);
            if (!string.IsNullOrEmpty(existingReview.ReviewText))
            {
                existingReview.ReviewText = reviewText;
                existingReview.DateReviewed = DateTime.UtcNow;
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
                _context.AlbumReviews.Add(newReview);
            }
            await _context.SaveChangesAsync();
        }
        public async Task RemoveReviewAsync(int albumId, string userName)
        {
            var review = await GetReviewAsync(albumId, userName);
            if (review != null)
            {
                _context.AlbumReviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
    
}