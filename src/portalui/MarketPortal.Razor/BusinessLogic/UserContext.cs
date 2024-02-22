using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace MarketPortal.Razor.BusinessLogic
{
    public interface IUserContext
    {
        AccessToken AccessToken { get; set; }
        string BearerToken { get; set; }

        Task<string> GetBearerToken();
    }

    public class UserContext : IUserContext
    {
        IAccessTokenProvider accessTokenProvider;
        public UserContext(IAccessTokenProvider _accessTokenProvider)
        {
            accessTokenProvider = _accessTokenProvider;
        }
        public string BearerToken { get; set; }
        public AccessToken AccessToken { get; set; }

        public async Task<string> GetBearerToken()
        {
            await CheckTocken().ConfigureAwait(false);
            return "Bearer " + AccessToken.Value.ToString();
        }

        async Task CheckTocken()
        {
            var accessTokenResult = await accessTokenProvider.RequestAccessToken();

            if (!accessTokenResult.TryGetToken(out var token))
            {
                throw new InvalidOperationException(
                    "Failed to provision the access token.");
            }

            AccessToken = token;
        }
    }
}
