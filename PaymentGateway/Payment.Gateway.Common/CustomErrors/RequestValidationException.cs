using System;

namespace Payment.Gateway.Common.CustomErrors
{
    public class RequestValidationException : Exception
    {
        public RequestValidationException(string message) : base(message)
        {

        }
    }
}
