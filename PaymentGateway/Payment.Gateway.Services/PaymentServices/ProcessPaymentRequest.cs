using System;

namespace Payment.Gateway.Services.PaymentServices
{
    public class ProcessPaymentRequest
    {
        public string CardNumber { get; set; }
        public string CardCvv { get; set; }
        public string ExpiryDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Currency { get; set; }
    }
}
