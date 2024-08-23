using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumDatabaseServer.Data
{
    public class UserProfilePicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [Required]
        public string PicturePath { get; set; }
    }
}
