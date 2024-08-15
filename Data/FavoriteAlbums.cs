using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumDatabaseServer.Data
{
	public class FavoriteAlbums
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int FavoriteId { get; set; }

		[ForeignKey("UserName")]
		public string UserName { get; set; }
		public IdentityUser User { get; set; }
		
		[ForeignKey("AlbumId")]		
		public int AlbumId { get; set; }
		public Album Album { get; set; }

		public DateTime DateFavorited { get; set; }
	}
}
