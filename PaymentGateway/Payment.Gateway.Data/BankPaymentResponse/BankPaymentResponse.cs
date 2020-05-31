using System;

namespace Payment.Gateway.Data.BankPaymentResponse
{
    public class BankPaymentResponse
    {
        public int Id { get; set; }
        public int PaymentRequestId { get; set; }
        public Guid? BankPaymentIdentifier { get; set; }
        public string Status { get; set; }
        public DateTime DateTimeAdded { get; set; }
    }
}
