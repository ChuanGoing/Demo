using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ChuanGoing.AuthorizationServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            services.AddIdentityServer()//Ids4服务
                                        //.AddDeveloperSigningCredential()
                .AddSigningCredential(new X509Certificate2(Path.Combine(basePath,
                Configuration["Certificates:CerPath"]),
                Configuration["Certificates:Password"]))
               .AddInMemoryApiResources(AuthConfig.GetApiResources())
               .AddInMemoryIdentityResources(AuthConfig.GetIdentityResources())
               .AddInMemoryClients(AuthConfig.GetClients(Configuration)) //把配置文件的Client配置资源放到内存
               .AddTestUsers(AuthConfig.GetUsers());
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Use(async (context, next) =>
            {
                // 这里从url中获取token参数，实际应用请实际考虑，加一些过滤条件
                //if (context.Request.Path.StartsWithSegments(path) && context.Request.Query.TryGetValue("access_token", out var token))
                //{
                //    // 从url中拿到header，再添加到header中，一定要在UseAuthentication之前
                //    context.Request.Headers.Add("Authorization", $"Bearer {token}");
                //}
                await next.Invoke();
            });
        }
    }
}
