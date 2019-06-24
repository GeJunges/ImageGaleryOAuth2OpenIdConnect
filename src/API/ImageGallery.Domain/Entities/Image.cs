using System.ComponentModel.DataAnnotations;

namespace ImageGallery.Domain.Entities
{
    public class Image: IEntity
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(50)]
        public string OwnerId { get; set; }
    }
}
