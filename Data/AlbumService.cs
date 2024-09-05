using AlbumDatabaseServer.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AlbumService
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
	private List<Album> _albums = new();
	public event EventHandler AlbumsChanged;
	public AlbumService(IDbContextFactory<ApplicationDbContext> dbFactory)
	{
		_dbContextFactory = dbFactory;
		_ = LoadAlbumsAsync();
	}
	private ApplicationDbContext CreateContext() => _dbContextFactory.CreateDbContext();
	public List<Album> Albums => _albums;
	private async Task LoadAlbumsAsync()
	{
		using var context = CreateContext();
		var newAlbums = await context.Albums
			.Include(a => a.Artist)
			.Include(a => a.AlbumGenres)
			.ThenInclude(ag => ag.Genre)
			.Include(a => a.Songs)
			.ToListAsync();
		_albums.Clear();
		_albums.AddRange(newAlbums);
		NotifyStateChanged();
	}
	public async Task RefreshAlbumsAsync()
	{
		await LoadAlbumsAsync();  // Call the private method internally
	}
	public async Task AddAlbum(Album newAlbum)
	{
		using var context = CreateContext();
		context.Albums.Add(newAlbum);
		await context.SaveChangesAsync();
		await LoadAlbumsAsync();
	}
	public void NotifyStateChanged() => AlbumsChanged?.Invoke(this, EventArgs.Empty);
	public List<Album> GetNewReleases()
	{
        using var context = CreateContext();
		var thresholdNew = DateTime.UtcNow.AddMonths(-3);	// threshold for new qualification (past 3 months)
		return context.Albums
			.Include(a => a.Artist)
			.Where(a => a.ReleaseDate >= thresholdNew)
			.OrderByDescending(a => a.ReleaseDate)
			.ToList();
	}
    public decimal GetAverageRating(int albumId)
    {
        using var context = CreateContext();
		var ratings = context.AlbumRatings
			.Where(r => r.AlbumId == albumId)
			.Select(r => (decimal)r.Rating);
		return ratings.Any() ? ratings.Average() : 0M;
	}
	public int GetRatingCount(int albumId)
    {
        using var context = CreateContext();
        return context.AlbumRatings
            .Count(r => r.AlbumId == albumId);
    }
	public async Task<List<Album>> SearchAlbumsAsync(string searchQuery)
	{
		if (string.IsNullOrWhiteSpace(searchQuery))
		{
			return new List<Album>();
		}
		using var context = CreateContext();
		var loweredQuery = searchQuery.ToLower();
		var results = await context.Albums
			.Include(a => a.Artist)
			.Where(a => a.Name.ToLower().Contains(loweredQuery)
				|| a.Artist.ArtistName.ToLower().Contains(loweredQuery))
			.ToListAsync();
		return results;
	}
	public async Task<List<Album>> GetAlbumsByArtistAsync(int artistId)
	{
		using var context = CreateContext();
		var albums = await context.Albums
			.Where(a => a.Artist.ArtistId == artistId)
			.Include(a => a.Artist)
			.Include(a => a.AlbumGenres)
			.ThenInclude(ag => ag.Genre)
			.Include(a => a.Songs)
			.ToListAsync();
		return albums;
	}
	public async Task<Artist> GetArtistAsync(int artistId)
	{
		using var context = CreateContext();
		var artist = await context.Artists
			.Where(a => a.ArtistId == artistId)
			.SingleOrDefaultAsync();
		return artist;
	}
	public async Task<Artist> GetArtistByNameAsync(string artistName)
	{
		using var context = CreateContext();
		var artist = await context.Artists
			.Where(a => a.ArtistName == artistName)
			.SingleOrDefaultAsync();
		return artist;
	}
	public async Task<bool> ArtistNameExistsAsync(string artistName) // returns true if artist name exists in db, else false
	{
		using var context = CreateContext();
		var artist = await context.Artists
			.Where(a => a.ArtistName == artistName)
			.SingleOrDefaultAsync();
		if (artist == null)
		{
			return false;
		}
		return true;
	}
	public async Task<bool> GenreNameExistsAsync(string genreName) // returns true if genre name exists in db, else false
	{
		using var context = CreateContext();
		var genre = await context.Genres
			.Where(g => g.Name == genreName)
			.SingleOrDefaultAsync();
		if (genre == null)
		{
			return false;
		}
		return true;
	}
	public async Task<int> GetGenreIdByNameAsync(string genreName)
	{
		using var context = CreateContext();
		var genre = await context.Genres
			.Where(g => g.Name == genreName)
			.SingleOrDefaultAsync();
		if (genre == null)
		{
			return -1;
		}
		return genre.GenreId;
	}
	public async Task<bool> AlbumExistsAsync(string albumName, int artistId)
	{
		using var context = CreateContext();
		var album = await context.Albums
			.Include(a => a.Artist)
			.Where(a => a.Name == albumName && a.Artist.ArtistId == artistId)
			.SingleOrDefaultAsync();
        if (album == null)
        {
			return false;
        }
		return true;
    }
}
