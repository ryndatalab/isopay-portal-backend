using System.Threading;

namespace MarketPortal.Razor.BusinessLogic
{
    public class HttpClientCustom
    {
        private IUserContext _userContext ;
        public HttpClient HttpClient { get; set; }
        public HttpClientCustom(IUserContext userContext)
        {
            _userContext = userContext;
            HttpClient.DefaultRequestHeaders.Add("Authentication", userContext.BearerToken);
        } 

    }
}
