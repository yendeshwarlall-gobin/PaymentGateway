namespace Payment.Gateway.Data.PaymentRequest
{
    public interface IPaymentRequestDao
    {
        void InsertPaymentRequest(PaymentRequest request);
        PaymentRequest GetPaymentRequestBasedOnMerchantIdCardIdCurrencyIdAndAmount(PaymentRequest request);
    }
}
