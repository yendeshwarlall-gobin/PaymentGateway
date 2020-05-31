using Payment.Gateway.Security.Authentication;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Payment.Gateway.Data.Merchant;

namespace Payment.Gateway.Web.Api.CustomHandlers
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly AuthenticationService _authenticationService = new AuthenticationService(new MerchantDao());

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.TryGetValues("X-ApiKey", out var apiKeyHeaderValues))
            {
                var apiKeyHeaderValue = apiKeyHeaderValues.First();

                var merchant = _authenticationService.GetMerchantBasedOnApiKey(apiKeyHeaderValue);

                if (merchant != null)
                {
                    var userIdClaim = new Claim(ClaimTypes.Name, merchant.ApiKey.ToString());
                    var identity = new ClaimsIdentity(new[] { userIdClaim }, "ApiKey");
                    var principal = new ClaimsPrincipal(identity);

                    Thread.CurrentPrincipal = principal;

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = principal;
                    }
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}