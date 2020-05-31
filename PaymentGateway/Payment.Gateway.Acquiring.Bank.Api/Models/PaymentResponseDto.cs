
using System;

namespace Payment.Gateway.Acquiring.Bank.Api.Models
{
    public class PaymentResponseDto
    {
        public Guid? PaymentIdentifier { get; set; }
        public string PaymentStatus { get; set; }
    }
}