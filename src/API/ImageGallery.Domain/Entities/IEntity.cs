using System;
using System.ComponentModel.DataAnnotations;

namespace ImageGallery.Domain.Entities
{
    public class IEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
