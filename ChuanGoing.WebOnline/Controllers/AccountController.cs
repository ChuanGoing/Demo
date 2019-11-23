using ChuanGoing.WebOnline.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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

        public async Task<IActionResult> LoginProduct(UserRequestModel model)
        {
            var client = _httpClientFactory.CreateClient("AuthenClient");

            DiscoveryResponse disco = await client.GetDiscoveryDocumentAsync();
            if (disco.IsError)
            {
                return new JsonResult(new { err = disco.Error });
            }

            TokenResponse token = null;
            switch (model.Type.ToLower())
            {
                case "client":
                    token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
                    {
                        //获取Token的地址
                        Address = disco.TokenEndpoint,
                        //客户端Id
                        ClientId = "ClientCredentials",
                        //客户端密码
                        ClientSecret = "ClientSecret",
                        //要访问的api资源
                        Scope = "ProductApi"
                    });
                    break;
                case "password":
                    token = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
                    {
                        //获取Token的地址
                        Address = disco.TokenEndpoint,
                        //客户端Id
                        ClientId = "ResourceOwnerPassword",
                        //客户端密码
                        ClientSecret = "ClientSecret",
                        //要访问的api资源
                        Scope = "ProductApi",
                        UserName = model.Name,
                        Password = model.Password
                    });
                    break;
                case "code":
                    token = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest()
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = "GrantCode",
                        //客户端密码
                        ClientSecret = "CodeSecret",
                        Code = model.Code,
                        RedirectUri = "http://localhost:5000/Home"
                    });
                    break;
            }
            if (token.IsError)
                return new JsonResult(new { err = token.Error });
            client.SetBearerToken(token.AccessToken);
            string data = await client.GetStringAsync("http://localhost:7004/api/values");
            JArray json = JArray.Parse(data);
            return new JsonResult(json);
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
