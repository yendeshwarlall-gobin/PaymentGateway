using Payment.Gateway.Data.Merchant;

namespace Payment.Gateway.Security.Authentication
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IMerchantDao _merchantDao;

        public AuthenticationService(IMerchantDao merchantDao)
        {
            _merchantDao = merchantDao;
        }

        public Merchant GetMerchantBasedOnApiKey(string apiKey)
        {
            return _merchantDao.GetMerchantBasedOnApiKey(apiKey);
        }
    }
}
