using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            string accessToken;
            var currentContext = HttpContextAccessor.HttpContext;
            //accessToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var expires_at = await currentContext.GetTokenAsync("expires_at");

            if (string.IsNullOrWhiteSpace(expires_at) || ((DateTime.Parse(expires_at).AddSeconds(-60)).ToUniversalTime() < DateTime.UtcNow))
            {
                accessToken = await RenewTokens();
            }
            else
            {
                accessToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            }

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                HttpClient.SetBearerToken(accessToken);
            }

            HttpClient.BaseAddress = new Uri("https://localhost:5001/");
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return HttpClient;
        }

        private async Task<string> RenewTokens()
        {
            var currentContext = HttpContextAccessor.HttpContext;

            var discoveryClient = new DiscoveryClient("https://localhost:5003/");
            var metaDataResponse = await discoveryClient.GetAsync();

            var tokenClient = new TokenClient(metaDataResponse.TokenEndpoint, "imagegalleryclient", "secret");
            var currentRefreshToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            var tokenResult = await tokenClient.RequestRefreshTokenAsync(currentRefreshToken);

            if (!tokenResult.IsError)
            {
                var expireAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);

                var updateTokens = new List<AuthenticationToken> {
                    new AuthenticationToken {
                        Name = OpenIdConnectParameterNames.IdToken,
                        Value = tokenResult.IdentityToken,
                    },
                    new AuthenticationToken {
                        Name = OpenIdConnectParameterNames.AccessToken,
                        Value = tokenResult.AccessToken,
                    },
                    new AuthenticationToken {
                        Name = OpenIdConnectParameterNames.RefreshToken,
                        Value = tokenResult.RefreshToken,
                    },
                    new AuthenticationToken {
                        Name = "expires_at",
                        Value = expireAt.ToString("o", CultureInfo.InvariantCulture)
                    }
                };

                var currentAuthenticateResult = await currentContext.AuthenticateAsync("Cookies");
                currentAuthenticateResult.Properties.StoreTokens(updateTokens);

                await currentContext.SignInAsync("Cookies", currentAuthenticateResult.Principal, currentAuthenticateResult.Properties);

                return tokenResult.AccessToken;
            }
            else
            {
                throw new Exception("Problem encountered while refreshing tokens.", tokenResult.Exception);
            }
        }
    }
}
