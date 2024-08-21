using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                .FirstOrDefaultAsync(r => r.AlbumId == albumId && r.UserName == userName);
        }
        public async Task SubmitAlbumRatingAsync(int albumId, string userName, int rating, string review)
        {
            var albumRating = await GetRatingAsync(albumId, userName);
            if (albumRating != null)
            {
                albumRating.Rating = rating;
                albumRating.Review = review;
                albumRating.DateRated = DateTime.UtcNow;
            }
            else
            {
                albumRating = new AlbumRating
                {
                    AlbumId = albumId,
                    UserName = userName,
                    Rating = rating,
                    Review = review,
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
    }
    
}