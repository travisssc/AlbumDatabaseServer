using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumDatabaseServer.Data
{
    public class AlbumLists
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListId { get; set; }
        public string Description { get; set; }
        [Required]
        public string UserId { get; set; }
        public string ListName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
		public List<ListAlbum> ListAlbums { get; set; } = new List<ListAlbum>();
	}
}
