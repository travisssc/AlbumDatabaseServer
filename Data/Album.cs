using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumDatabaseServer.Data
{
    public class Album
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlbumId { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int? MediaType { get; set; }
        public string AlbumCover {  get; set; } = string.Empty;
        public List<Song> Songs { get; set; } = new List<Song>();
        public List<AlbumGenre> AlbumGenres { get; set; }

        [ForeignKey("ArtistId")]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
    }
}