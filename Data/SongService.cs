namespace AlbumDatabaseServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SongService
{
	private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
	private List<Song> _songs;
	public SongService(IDbContextFactory<ApplicationDbContext> dbFactory)
	{
		_dbFactory = dbFactory;
		_songs = new List<Song>();
	}
	public List<Song> GetSongsFromAlbum(int albumId)
	{
		using var context = _dbFactory.CreateDbContext();
		return context.Songs
			.Where(s => s.AlbumId == albumId)
			.OrderBy(s => s.SongPosition)
			.ToList();
	}
	public async Task UpdateSongPositions(List<Song> songs)
	{
		using var context = _dbFactory.CreateDbContext();
		foreach (Song s in songs)
		{
			context.Update(s);
		}
		await context.SaveChangesAsync();
	}
	public async Task DeleteSongAsync(int songId)
	{
		using var context = _dbFactory.CreateDbContext();
		var songToDelete = await context.Songs.FindAsync(songId);
		if (songToDelete != null)
		{
			context.Songs.Remove(songToDelete);
			await context.SaveChangesAsync();
		}
    }
}
