using AlbumDatabaseServer.Data;
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
    public async Task AddTestRating(int albumId, string userName, int rating, string review = "")
    {
        using var context = _dbFactory.CreateDbContext();

        var user = context.Users.FirstOrDefault(u => u.UserName == userName);
        if (user == null)
        {
            // Handle case where user does not exist
            return;
        }

		var albumRating = new AlbumRating
		{
			AlbumId = albumId,
			UserName = userName,
			User = user,
			Rating = rating,
			Review = review,
			DateRated = DateTime.Now.ToUniversalTime()
		};

        context.AlbumRatings.Add(albumRating);
        await context.SaveChangesAsync();
    }
}
