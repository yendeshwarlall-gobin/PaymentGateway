namespace Payment.Gateway.Data.Merchant
{
    public interface IMerchantDao
    {
        Data.Merchant.Merchant GetMerchantBasedOnApiKey(string apiKey);
    }
}
