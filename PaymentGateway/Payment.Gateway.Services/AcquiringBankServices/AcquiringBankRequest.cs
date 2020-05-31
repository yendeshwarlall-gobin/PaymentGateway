
using System;

namespace Payment.Gateway.Services.AcquiringBankServices
{
    public class AcquiringBankRequest
    {
        public int MerchantId { get; set; }
        public string CardNumber { get; set; }
        public string CardCvv { get; set; }
        public string ExpiryDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Currency { get; set; }
    }
}
