﻿using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ChuanGoing.AuthorizationServer
{
    public class AuthConfig
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("WebApi", "ChuanGoingWebApi"),
                new ApiResource("ProductApi", "ChuanGoingWebProduct")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration Configuration)
        {
            var OnlineConfig = Configuration.GetSection("OnlineClient");
            var List = new List<Client>
            {
                new Client()
                {
                    ClientId = "ClientCredentials",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("ClientSecret".Sha256()) },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "WebApi",
                        "ProductApi"
                    },
                    AccessTokenLifetime = 10 * 60 * 1
                },

                new Client()
                {
                    ClientId = "ResourceOwnerPassword",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("ClientSecret".Sha256()) },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "WebApi",
                        "ProductApi"
                    },
                    AccessTokenLifetime = 10 * 60 * 1
                },
                  /*
                  隐式模式:https://localhost:6005/connect/authorize?client_id=Implicit&redirect_uri=http://localhost:5000/Home&response_type=token&scope=WebApi
                  */
                new Client()
                {
                    ClientId = "Implicit",
                    ClientName = "ImplicitClient",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets = { new Secret("ImplicitSecret".Sha256()) },
                    RedirectUris ={OnlineConfig.GetValue<string>("RedirectUris") },
                    PostLogoutRedirectUris = {OnlineConfig.GetValue<string>("LogoutRedirectUris") },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "WebApi",
                        "ProductApi"
                    },
                    AccessTokenLifetime = 10 * 60 * 1,
                    //允许将token通过浏览器传递
                     AllowAccessTokensViaBrowser=true
                },
                /*
                 * 授权码模式:https://localhost:6005/connect/authorize?client_id=GrantCode&redirect_uri=http://localhost:5000/Home&response_type=code&scope=WebApi
                 */
                new Client()
                {
                   //客户端Id
                    ClientId="GrantCode",
                    ClientName="GrantCodeClient",
                    //客户端密码
                    ClientSecrets={new Secret("CodeSecret".Sha256()) },
                    //客户端授权类型，Code:授权码模式
                    AllowedGrantTypes=GrantTypes.Code,
                    //允许登录后重定向的地址列表，可以有多个
                     RedirectUris ={OnlineConfig.GetValue<string>("RedirectUris") }, 
                    //允许访问的资源
                    AllowedScopes={
                        "WebApi",
                        "ProductApi"
                    }
                }
            };
            return List;
        }

        //测试用户
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "admin",
                    Password = "123456"

                    //Claims = new List<Claim>
                    //{
                    //    new Claim("name", "admin"),
                    //    new Claim("website", "https://www.cnblogs.com/chuangoing")
                    //}
                },
                new TestUser
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "chuangoing",
                    Password = "123456"

                    //Claims = new List<Claim>
                    //{
                    //    new Claim("name", "chuangoing"),
                    //    new Claim("website", "https://github.com/chuangoing")
                    //}
                }
            };
        }
    }
}
