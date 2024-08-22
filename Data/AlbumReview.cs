using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumDatabaseServer.Data
{
    public class AlbumReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        [ForeignKey("UserName")]
        public string UserName { get; set; }
        public IdentityUser User { get; set; }

        [ForeignKey("AlbumId")]
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        [Required]
        public string ReviewText { get; set; }
        public DateTime DateReviewed { get; set; }
    }
}
