using System;

namespace Payment.Gateway.Data.BankPaymentResponse
{
    public class PaymentDetail
    {
        public Guid PaymentIdentifier { get; set; }
        public string CardNumber { get; set; }
        public string CardCVV { get; set; }
        public string Currency { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }
    }
}
