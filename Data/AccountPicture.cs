using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AlbumDatabaseServer.Data
{
	public class AccountPicture
	{
		[Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int PictureId { get; set; }
		[ForeignKey("UserName")]
		public string UserName { get; set; }
		public IdentityUser User { get; set; }
		public string PicturePath { get; set; }

	}
}
