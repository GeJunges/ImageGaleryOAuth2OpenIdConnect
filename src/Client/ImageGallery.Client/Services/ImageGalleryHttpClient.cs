using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageGallery.Client.Services
{
    public class ImageGalleryHttpClient : IImageGalleryHttpClient
    {
        private IHttpContextAccessor httpContextAccessor { get; set; }
        private HttpClient HttpClient { get; set; }

        public ImageGalleryHttpClient(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            HttpClient = new HttpClient();
        }

        public async Task<HttpClient> GetClient()
        {
            HttpClient.BaseAddress = new Uri("https://localhost:5001");
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return HttpClient;
        }
    }
}
