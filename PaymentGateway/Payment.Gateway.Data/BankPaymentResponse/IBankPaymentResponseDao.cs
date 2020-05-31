using System;

namespace Payment.Gateway.Data.BankPaymentResponse
{
    public interface IBankPaymentResponseDao
    {
        void InsertBankPaymentResponse(BankPaymentResponse bankPaymentResponse);
        PaymentDetail GetBankPaymentResponse(Guid bankPaymentIdentifier);
    }
}
