using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlbumDatabaseServer.Data
{
    public class ListAlbum
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListAlbumId { get; set; }

        [Required]
        public int Position { get; set; }

        [ForeignKey("ListId")]
        public int ListId { get; set; }
        public AlbumLists List { get; set; }
        [ForeignKey("AlbumId")]
        public int AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
