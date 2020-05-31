using System;

namespace Payment.Gateway.Data.Cards
{
    public class Card
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public string CVV { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
