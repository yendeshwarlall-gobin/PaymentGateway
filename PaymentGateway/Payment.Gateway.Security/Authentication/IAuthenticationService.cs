using Payment.Gateway.Data.Merchant;

namespace Payment.Gateway.Security.Authentication
{
    public interface IAuthenticationService
    {
        Merchant GetMerchantBasedOnApiKey(string apiKey);
    }
}
