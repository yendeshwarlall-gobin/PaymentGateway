using System;
using Payment.Gateway.Data.BankPaymentResponse;

namespace Payment.Gateway.Services.PaymentServices
{
    public interface IPaymentService
    {
        PaymentResponse ProcessPayment(ProcessPaymentRequest request, string merchantApiKey);
        PaymentDetail GetPaymentDetail(string bankPaymentIdentifier);
    }
}
