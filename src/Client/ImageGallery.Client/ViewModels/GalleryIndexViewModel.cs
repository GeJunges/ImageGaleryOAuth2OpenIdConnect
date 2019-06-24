using System.Collections.Generic;

namespace ImageGallery.Client.ViewModels
{
    public class GalleryIndexViewModel
    {
        public IEnumerable<Model.Image> Images { get; private set; } = new List<Model.Image>();

        public GalleryIndexViewModel(List<Model.Image> images)
        {
            Images = images;
        }
    }
}
