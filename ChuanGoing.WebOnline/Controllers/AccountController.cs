using ChuanGoing.WebOnline.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChuanGoing.WebOnline.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> LoginProduct(UserRequestModel userRequestModel)
        {
            var client = _httpClientFactory.CreateClient("AuthenClient");

            DiscoveryResponse disco = await client.GetDiscoveryDocumentAsync();
            if (disco.IsError)
            {
                return "401(认证服务器未启动)";
            }
            TokenResponse tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "ResourceOwnerPassword",
                ClientSecret = "ClientSecret",
                UserName = userRequestModel.Name,
                Password = userRequestModel.Password,
                Scope = "ProductApi"
            });

            return tokenResponse.IsError ? tokenResponse.Error : tokenResponse.AccessToken;
        }

        public async Task<string> LoginOrder(UserRequestModel userRequestModel)
        {
            var client = _httpClientFactory.CreateClient("AuthenClient");

            DiscoveryResponse disco = await client.GetDiscoveryDocumentAsync();
            if (disco.IsError)
            {
                return "401(认证服务器未启动)";
            }
            TokenResponse tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "ClientCredentials",
                ClientSecret = "ClientSecret",
                Scope = "WebApi"
            });

            return tokenResponse.IsError ? tokenResponse.Error : tokenResponse.AccessToken;
        }
    }
}
