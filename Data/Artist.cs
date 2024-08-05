using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumDatabaseServer.Data
{
    public class Artist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArtistId {  get; set; }
        [Required]
        public string ArtistName { get; set; } = string.Empty;
        public string ArtistDescription { get; set; } = string.Empty;
        public string ArtistImage {  get; set; } = string.Empty;
        public int[] Members { get; set; } = new int[0];

    }
}