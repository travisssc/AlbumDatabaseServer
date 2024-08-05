using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumDatabaseServer.Data
{
    public class Song
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SongId { get; set; }
        [Required]
        public string SongName { get; set; }
        [Required]
        public int SongPosition { get; set; }
        [ForeignKey("AlbumId")]
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        [ForeignKey("ArtistId")]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        
    }
}