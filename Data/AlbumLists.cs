using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumDatabaseServer.Data
{
    public class AlbumLists
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ListName { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
        public List<ListAlbum> ListAlbums { get; set; }
    }
    public class ListAlbum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListAlbumId { get; set; }
        [Required]
        public int ListId { get; set; }
        [ForeignKey("ListId")]
        public AlbumLists List { get; set; }
        [Required]
        public int AlbumId { get; set; }
        [ForeignKey("AlbumId")]
        public Album Album { get; set; }
        public int Position { get; set; }
    }
}
