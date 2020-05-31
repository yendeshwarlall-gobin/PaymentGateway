
using System;

namespace Payment.Gateway.Services.AcquiringBankServices
{
    public class AcquiringBankResponse
    {
        public Guid? PaymentIdentifier { get; set; }
        public string PaymentStatus { get; set; }
    }
}
