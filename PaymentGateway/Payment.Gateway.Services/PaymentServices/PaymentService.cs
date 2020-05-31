using System;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using NLog;
using Payment.Gateway.Common.CustomErrors;
using Payment.Gateway.Data.BankPaymentResponse;
using Payment.Gateway.Data.Cards;
using Payment.Gateway.Data.Currency;
using Payment.Gateway.Data.Merchant;
using Payment.Gateway.Data.PaymentRequest;
using Payment.Gateway.Services.AcquiringBankServices;

namespace Payment.Gateway.Services.PaymentServices
{
    public class PaymentService: IPaymentService
    {
        private readonly ICardDao _cardDao;
        private readonly ICurrencyDao _currencyDao;
        private readonly IPaymentRequestDao _paymentRequestDao;
        private readonly IMerchantDao _merchantDao;
        private readonly IAcquiringBankService _acquiringBankService;
        private readonly IBankPaymentResponseDao _bankPaymentResponseDao;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _encryptionKey;
        private readonly string _expiryDateFormat = "yyyy-MM";

        public PaymentService(ICardDao cardDao, ICurrencyDao currencyDao, IPaymentRequestDao paymentRequestDao,
            IMerchantDao merchantDao, IAcquiringBankService acquiringBankService,
            IBankPaymentResponseDao bankPaymentResponseDao)
        {
            _cardDao = cardDao;
            _currencyDao = currencyDao;
            _paymentRequestDao = paymentRequestDao;
            _merchantDao = merchantDao;
            _acquiringBankService = acquiringBankService;
            _bankPaymentResponseDao = bankPaymentResponseDao;
            _encryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
        }

        public PaymentResponse ProcessPayment(ProcessPaymentRequest request, string merchantApiKey)
        {
            //Get merchant
            var merchant = _merchantDao.GetMerchantBasedOnApiKey(merchantApiKey);
            if (merchant == null)
            {
                Logger.Info("Request from an unknown merchant received");
                throw new RequestValidationException("Unknown Merchant");
            }

            if (string.IsNullOrWhiteSpace(request.CardNumber))
            {
                Logger.Info($"Bad Request from MerchantId :{merchant.ID}, Missing Card Number");
                throw new RequestValidationException("Missing Card Number");
            }

            if (!(request.CardNumber.Length == 19 || request.CardNumber.Length == 16))
            {
                Logger.Info($"Bad Request from MerchantId :{merchant.ID}, Invalid Card Number");
                throw new RequestValidationException("Invalid Card Number");
            }

            if (string.IsNullOrWhiteSpace(request.CardCvv))
            {
                Logger.Info($"Bad Request from MerchantId :{merchant.ID}, Missing Card CVV Number");
                throw new RequestValidationException("Missing Card CVV Number");
            }

            if ( request.CardCvv.Length != 4)
            {
                Logger.Info($"Bad Request from MerchantId :{merchant.ID}, Invalid Card Card CVV Number");
                throw new RequestValidationException("Invalid Card Card CVV Number");
            }

            if (request.PaymentAmount == 0)
            {
                Logger.Info($"Bad Request from MerchantId :{merchant.ID}, Invalid Payment Amount");
                throw new RequestValidationException("Payment Amount should be greater than zero");
            }

            if (string.IsNullOrWhiteSpace(request.ExpiryDate))
            {
                Logger.Info($"Bad Request from MerchantId :{merchant.ID}, Missing Card Expiry Date");
                throw new RequestValidationException("Missing Card Card Expiry Date");
            }

            DateTime expiryDate;

            try
            {
                expiryDate = DateTime.ParseExact(request.ExpiryDate, _expiryDateFormat, CultureInfo.InvariantCulture);

            }
            catch (Exception e)
            {
                Logger.Info($"Bad Request from MerchantId :{merchant.ID}, Invalid ExpiryDate provided");
                throw new RequestValidationException("Invalid ExpiryDate provided");
            }

            if (string.IsNullOrWhiteSpace(request.Currency))
            {
                Logger.Info($"Bad Request from MerchantId :{merchant.ID}, Missing Currency");
                throw new RequestValidationException("Missing Currency");
            }



            var currency = GetCurrency(request);

            if (currency == null)
            {
                Logger.Info($"Bad Request from MerchantId :{merchant.ID}, Unknown currency");
                throw new RequestValidationException("Unknown Currency");
            }



            var card = GetOrCreateCard(request,expiryDate);

            var paymentRequest = new PaymentRequest
            {
                MerchantId = merchant.ID,
                CardId = card.ID,
                CurrencyId = currency.ID,
                Amount = request.PaymentAmount,
                DateTimeAdded = DateTime.UtcNow
            };

            _paymentRequestDao.InsertPaymentRequest(paymentRequest);

            paymentRequest =
                _paymentRequestDao.GetPaymentRequestBasedOnMerchantIdCardIdCurrencyIdAndAmount(paymentRequest);
            Logger.Info($"Payment request from Merchant: {merchant.ID} logged. PaymentRequestId : {paymentRequest.Id}");

            var acquiringBankRequest = new  AcquiringBankRequest
            {
                CardNumber = request.CardNumber, 
                CardCvv = request.CardCvv, 
                PaymentAmount = request.PaymentAmount, 
                ExpiryDate = request.ExpiryDate, 
                Currency = request.Currency, 
                MerchantId = merchant.ID
            };
             var response= _acquiringBankService.ProcessPayment(acquiringBankRequest);

             var bankPaymentResponse = new BankPaymentResponse
             {
                 PaymentRequestId = paymentRequest.Id,
                 Status = response.PaymentStatus,
                 BankPaymentIdentifier = response.PaymentIdentifier,
                 DateTimeAdded = DateTime.UtcNow
             };

            _bankPaymentResponseDao.InsertBankPaymentResponse(bankPaymentResponse);

            return new PaymentResponse
            {
                Status = response.PaymentStatus, 
                PaymentUniqueId = response.PaymentIdentifier
            };
        }

        public PaymentDetail GetPaymentDetail(string bankPaymentIdentifier)
        {
            if(string.IsNullOrWhiteSpace(bankPaymentIdentifier))
                Logger.Info($"Bank Payment Identifier not provided");

            Guid.TryParse(bankPaymentIdentifier, out var guidBankPaymentIdentifier);

            var paymentDetail = _bankPaymentResponseDao.GetBankPaymentResponse(guidBankPaymentIdentifier);

            if (paymentDetail == null)
                return null;


            paymentDetail.CardNumber = GetEncryptedCardNumber(paymentDetail.CardNumber);

            return paymentDetail;
        }

        private Currency GetCurrency(ProcessPaymentRequest request)
        {
            var currency = _currencyDao.GetCurrencyBasedOnDescription(request.Currency);

            if (currency == null)
            {
                currency = _currencyDao.GetCurrencyBasedOnShortDescription(request.Currency);
            }

            return currency;
        }

        private Card GetOrCreateCard(ProcessPaymentRequest request, DateTime expiryDate)
        {
            var card = _cardDao.GetCardBasedOnNumberAndCVV(request.CardNumber,request.CardCvv);

            if (card == null)
            {
                card = new Card
                {
                    Number = request.CardNumber,
                    CVV = request.CardCvv,
                    ExpiryDate = expiryDate
                };

                _cardDao.InsertCardInfo(card);

                card = _cardDao.GetCardBasedOnNumberAndCVV(request.CardNumber, request.CardCvv);
                Logger.Info($"New card details added, CardId :{card.ID}");
                return card;
            }

            return card;
        }

        private string GetEncryptedCardNumber(string paymentDetailCardNumber)
        {
            byte[] SrctArray;

            byte[] EnctArray = UTF8Encoding.UTF8.GetBytes(paymentDetailCardNumber);

            SrctArray = UTF8Encoding.UTF8.GetBytes(_encryptionKey);

            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();

            MD5CryptoServiceProvider objcrpt = new MD5CryptoServiceProvider();

            SrctArray = objcrpt.ComputeHash(UTF8Encoding.UTF8.GetBytes(_encryptionKey));

            objcrpt.Clear();

            objt.Key = SrctArray;

            objt.Mode = CipherMode.ECB;

            objt.Padding = PaddingMode.PKCS7;

            ICryptoTransform crptotrns = objt.CreateEncryptor();

            byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);

            objt.Clear();

            return Convert.ToBase64String(resArray, 0, resArray.Length);
        }
    }
}
