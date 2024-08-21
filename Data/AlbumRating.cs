using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumDatabaseServer.Data
{
	public class AlbumRating
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RatingId { get; set; }

		[ForeignKey("UserName")]
		public string UserName { get; set; }
		public IdentityUser User { get; set; }

		[ForeignKey("AlbumId")]
		public int AlbumId { get; set; }
		public Album Album { get; set; }

		[Required]
		[Range(1, 5)]
		public int Rating { get; set; }

		public string Review { get; set; } = string.Empty;
		public DateTime DateRated { get; set; }
	}
}
