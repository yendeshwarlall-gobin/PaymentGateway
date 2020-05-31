using System;

namespace Payment.Gateway.Data.Merchant
{
    public class Merchant
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ApiKey { get; set; }
    }
}
