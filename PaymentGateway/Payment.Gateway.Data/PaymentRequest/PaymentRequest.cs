using System;

namespace Payment.Gateway.Data.PaymentRequest
{
    public class PaymentRequest
    {
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public int CardId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTimeAdded { get; set; }
    }
}
