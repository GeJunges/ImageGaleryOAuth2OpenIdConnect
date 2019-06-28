using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageGallery.Client.Services
{
    public class ImageGalleryHttpClient : IImageGalleryHttpClient
    {
        private IHttpContextAccessor HttpContextAccessor { get; set; }
        private HttpClient HttpClient { get; set; }

        public ImageGalleryHttpClient(IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContextAccessor = httpContextAccessor;
            HttpClient = new HttpClient();
        }

        public async Task<HttpClient> GetClient()
        {
            var currentContext = HttpContextAccessor.HttpContext;

            var accessToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                HttpClient.SetBearerToken(accessToken);
            }

            HttpClient.BaseAddress = new Uri("https://localhost:5001/");
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return HttpClient;
        }
    }
}
