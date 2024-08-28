using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AlbumDatabaseServer.Data;

namespace AlbumDatabaseServer.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //review \/
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AlbumGenre>()
                .HasKey(ag => new { ag.GenreId, ag.AlbumId });
            builder.Entity<AlbumGenre>()
                .HasOne(ag => ag.Genre)
                .WithMany(g => g.AlbumGenres)
                .HasForeignKey(ag => ag.GenreId);
            builder.Entity<AlbumGenre>()
                .HasOne(ag => ag.Album)
                .WithMany(a => a.AlbumGenres)
                .HasForeignKey(ag => ag.AlbumId);

        }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<AlbumGenre> AlbumGenres {  get; set; } 

        // User function tables
        public DbSet<AccountQueue> AccountQueue { get; set; }
        public DbSet<AlbumRating> AlbumRatings { get; set; }
        public DbSet<FavoriteAlbums> FavoriteAlbums { get; set; }
        public DbSet<ListenedAlbums> ListenedAlbums { get; set; }
        public DbSet<AlbumReview> AlbumReviews { get; set; }
        public DbSet<AlbumLists> Lists { get; set; }
        public DbSet<AccountPicture> AccountPictures { get; set; }
    }
}
