using MarketPortal.Razor;
using MarketPortal.Razor.BusinessLogic;
using MarketPortal.Razor.Services.Administration;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Json;

namespace MarketPortal.Razor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //builder.Services.AddHttpClient("BlazorKCLogin.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            //    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            // builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorKCLogin.ServerAPI"));


            builder.Services.AddOidcAuthentication(options =>
            {
                //options.ProviderOptions.MetadataUrl = "https://centralidp.preprod.cofinity-x.com/auth/realms/CX-Central/.well-known/openid-configuration";
                //options.ProviderOptions.Authority = "https://centralidp.preprod.cofinity-x.com/auth/realms/CX-Central";
                options.ProviderOptions.MetadataUrl = "http://localhost:8070/realms/CX-Central/.well-known/openid-configuration";
                options.ProviderOptions.Authority = "http://localhost:8070/realms/CX-Central";

                options.ProviderOptions.ClientId = "Cl2-CX-Portal";
                options.ProviderOptions.ResponseType = "code";

                //options.UserOptions.NameClaim = "name";
                //options.UserOptions.RoleClaim = "roles";
                options.UserOptions.ScopeClaim = "openid";
                options.ProviderOptions.ResponseMode = "fragment";
                options.ProviderOptions.DefaultScopes.Clear();
                options.ProviderOptions.DefaultScopes.Add("openid");
            });

            //builder.Services.AddMsalAuthentication(options =>
            //{
            //    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            //    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://api.id.uri/access_as_user");
            //});

            string adminAPIBaseUri = builder.Configuration.GetSection("AdminServiceConfig").GetValue<string>("BaseUri");
            //builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(adminAPIBaseUri) });

            //builder.Services.AddHttpClient("ProductsAPI", (sp, cl) =>
            //{
            //    cl.BaseAddress = new Uri("adminAPIBaseUri");
            //});

            //builder.Services.AddScoped(
            //    sp => sp.GetService<IHttpClientFactory>().CreateClient("ProductsAPI"));

            builder.Services.AddSingleton<IUserContext, UserContext>();
            //builder.Services.AddScoped(typeof( AdminAPI),  Span=> new AdminAPI(adminAPIBaseUri, new HttpClient { BaseAddress = new Uri(adminAPIBaseUri) }));
            //builder.Services.AddScoped(typeof(AdminAPI), Span => new AdminAPI(adminAPIBaseUri, new HttpClient { BaseAddress = new Uri(adminAPIBaseUri) }));
            builder.Services.AddSingleton(typeof(AdminAPI), Span => new AdminAPI(adminAPIBaseUri,
                new HttpClient { BaseAddress = new Uri(adminAPIBaseUri) } ));


            builder.Services.AddHttpClient();

            await builder.Build().RunAsync();
        }
    }
}
