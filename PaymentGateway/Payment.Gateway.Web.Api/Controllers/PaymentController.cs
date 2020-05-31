using System;
using System.Web.Http;
using NLog;
using Payment.Gateway.Common.CustomErrors;
using Payment.Gateway.Services.PaymentServices;

namespace Payment.Gateway.Web.Api.Controllers
{
    [Authorize]
    public class PaymentController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [Route("api/payment/process")]
        public IHttpActionResult Process(ProcessPaymentRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest();

                var principal = RequestContext.Principal;
                var response = _paymentService.ProcessPayment(request, principal.Identity.Name);

                return Ok(response);
            }
            catch (RequestValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch(Exception exception)
            {
                Logger.Error(exception);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/payment/detail")]
        public IHttpActionResult Detail([FromBody]string paymentIdentifier)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(paymentIdentifier))
                    return BadRequest();

                var principal = RequestContext.Principal;
                var response = _paymentService.GetPaymentDetail(paymentIdentifier);

                return Ok(response);
            }
            catch (RequestValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return InternalServerError(exception);
            }
        }
    }
}
