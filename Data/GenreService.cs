using AlbumDatabaseServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace AlbumDatabaseServer.Data
{
    public class GenreService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private List<Genre> _genres = new();
        public event EventHandler GenresChanged;
        public GenreService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
            _ = LoadGenresAsync();
        }
        public List<Genre> Genres => _genres;
        private async Task LoadGenresAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            var newGenres = await context.Genres
                .Include(g => g.AlbumGenres)
                .ThenInclude(ag => ag.Album)
                .ToListAsync();
            _genres.Clear();
            _genres.AddRange(newGenres);
            NotifyStateChanged();
        }
        public async Task RefreshGenresAsync()
        {
            await LoadGenresAsync();  // Call the private method internally
        }
        public async Task AddGenre(Genre newGenre)
        {
            using var context = _dbFactory.CreateDbContext();
            context.Genres.Add(newGenre);
            await context.SaveChangesAsync();
            await LoadGenresAsync();
        }
        public void NotifyStateChanged() => GenresChanged?.Invoke(this, EventArgs.Empty);
        public List<Genre> GetGenres()
        {
            using var context = _dbFactory.CreateDbContext();
            return context.Genres
                .Include(g => g.AlbumGenres)
                .ThenInclude(ag => ag.Album)
                .ToList();
        }
    }
}
