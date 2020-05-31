
using System;

namespace Payment.Gateway.Acquiring.Bank.Api.Models
{
    public class PaymentRequestDto
    {
        public int MerchantId { get; set; }
        public string CardNumber { get; set; }
        public string CardCvv { get; set; }
        public string ExpiryDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Currency { get; set; }
    }
}