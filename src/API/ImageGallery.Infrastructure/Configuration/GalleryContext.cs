using ImageGallery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageGallery.Infrastructure.Configuration
{
    public class GalleryContext : DbContext
    {
        public GalleryContext(DbContextOptions<GalleryContext> options) : base(options) { }
        public DbSet<Image> Images { get; set; }
    }
}
