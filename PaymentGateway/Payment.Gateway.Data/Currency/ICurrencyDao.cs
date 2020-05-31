namespace Payment.Gateway.Data.Currency
{
    public interface ICurrencyDao
    {
        Currency GetCurrencyBasedOnDescription(string description);
        Currency GetCurrencyBasedOnShortDescription(string shortDescription);
    }
}
