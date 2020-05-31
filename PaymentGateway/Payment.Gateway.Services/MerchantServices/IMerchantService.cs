using Payment.Gateway.Data.Merchant;

namespace Payment.Gateway.Services.MerchantServices
{
    public interface IMerchantService
    {
        Merchant GetMerchantBasedOnApiKey(string apiKey);
    }
}
