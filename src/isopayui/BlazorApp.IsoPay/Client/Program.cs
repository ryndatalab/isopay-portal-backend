using BlazorApp.IsoPay;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorApp.IsoPay
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddOidcAuthentication(options =>
            {
                //options.ProviderOptions.MetadataUrl = "https://centralidp.preprod.cofinity-x.com/auth/realms/isopay/.well-known/openid-configuration";
                //options.ProviderOptions.Authority = "https://centralidp.preprod.cofinity-x.com/auth/realms/isopay";
                options.ProviderOptions.MetadataUrl = "http://localhost:8070/realms/isopay/.well-known/openid-configuration";
                options.ProviderOptions.Authority = "http://localhost:8070/realms/isopay";

                options.ProviderOptions.ClientId = "isopay-portal";
                options.ProviderOptions.ResponseType = "code";

                //options.UserOptions.NameClaim = "name";
                //options.UserOptions.RoleClaim = "roles";
                options.UserOptions.ScopeClaim = "openid";
                options.ProviderOptions.ResponseMode = "fragment";
                options.ProviderOptions.DefaultScopes.Clear();
                options.ProviderOptions.DefaultScopes.Add("openid");
            });
            await builder.Build().RunAsync();
        }
    }
}
