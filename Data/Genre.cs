using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumDatabaseServer.Data
{
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GenreId { get; set; }
        [Required]
        public string Name { get; set; }
        public List<AlbumGenre> AlbumGenres { get; set; }
    }
}