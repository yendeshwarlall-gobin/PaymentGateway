using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Payment.Gateway.Acquiring.Bank.Api.Models;

namespace Payment.Gateway.Acquiring.Bank.Api.Controllers
{
    public class PaymentController : ApiController
    {
        // POST api/<controller>
        public HttpResponseMessage ProcessPayment(PaymentRequestDto request)
        {
            var result = new PaymentResponseDto();
            if (request.MerchantId > 2)
                return Request.CreateResponse(HttpStatusCode.BadRequest,"Unknown Merchant");

            if(request.PaymentAmount > 99999)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Payment amount exceeds allowed amount");

            return Request.CreateResponse(HttpStatusCode.OK, new PaymentResponseDto
            {
                PaymentIdentifier = Guid.NewGuid(),
                PaymentStatus = "Successful"
            });

        }
    }
}