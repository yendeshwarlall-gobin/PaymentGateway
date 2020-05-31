using System;

namespace Payment.Gateway.Services.PaymentServices
{
    public class PaymentResponse
    {
        public string Status { get; set; }
        public Guid? PaymentUniqueId { get; set; }
    }
}
