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
	private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
	private List<Album> _albums = new();
	public event EventHandler AlbumsChanged;
	public AlbumService(IDbContextFactory<ApplicationDbContext> dbFactory)
	{
		_dbFactory = dbFactory;
		_ = LoadAlbumsAsync();
	}
	public List<Album> Albums => _albums;
	private async Task LoadAlbumsAsync()
	{
		using var context = _dbFactory.CreateDbContext();
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
		using var context = _dbFactory.CreateDbContext();
		context.Albums.Add(newAlbum);
		await context.SaveChangesAsync();
		await LoadAlbumsAsync();
	}
	public void NotifyStateChanged() => AlbumsChanged?.Invoke(this, EventArgs.Empty);
	public List<Album> GetNewReleases()
	{
        using var context = _dbFactory.CreateDbContext();
        var thresholdNew = DateTime.UtcNow.AddMonths(-3);	// threshold for new qualification (past 3 months)
		return context.Albums
			.Include(a => a.Artist)
			.Where(a => a.ReleaseDate >= thresholdNew)
			.OrderByDescending(a => a.ReleaseDate)
			.ToList();
	}
    public decimal GetAverageRating(int albumId)
    {
        using var context = _dbFactory.CreateDbContext();
		var ratings = context.AlbumRatings
			.Where(r => r.AlbumId == albumId)
			.Select(r => (decimal)r.Rating);
		return ratings.Any() ? ratings.Average() : 0M;
	}
	public int GetRatingCount(int albumId)
    {
        using var context = _dbFactory.CreateDbContext();
        return context.AlbumRatings
            .Count(r => r.AlbumId == albumId);
    }
	public async Task<List<Album>> SearchAlbumsAsync(string searchQuery)
	{
		if (string.IsNullOrWhiteSpace(searchQuery))
		{
			return new List<Album>();
		}
		using var context = _dbFactory.CreateDbContext();
		var loweredQuery = searchQuery.ToLower();
		var results = await context.Albums
			.Include(a => a.Artist)
			.Where(a => a.Name.ToLower().Contains(loweredQuery)
				|| a.Artist.ArtistName.ToLower().Contains(loweredQuery))
			.ToListAsync();
		return results;
	}
}
