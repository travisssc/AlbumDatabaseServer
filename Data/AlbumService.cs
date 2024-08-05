using AlbumDatabaseServer.Data;
using Microsoft.EntityFrameworkCore;
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
}
