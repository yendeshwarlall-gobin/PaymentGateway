namespace Payment.Gateway.Services.AcquiringBankServices
{
    public interface IAcquiringBankService
    {
        AcquiringBankResponse ProcessPayment(AcquiringBankRequest request);
    }
}
