using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Payment.Gateway.Common.CustomErrors;
using Payment.Gateway.Data.BankPaymentResponse;
using Payment.Gateway.Data.Cards;
using Payment.Gateway.Data.Currency;
using Payment.Gateway.Data.Merchant;
using Payment.Gateway.Data.PaymentRequest;
using Payment.Gateway.Services.AcquiringBankServices;
using Payment.Gateway.Services.PaymentServices;

namespace Payment.Gateway.Web.Api.Tests
{
    [TestClass]
    public class PaymentServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(RequestValidationException))]
        public void Method_ProcessPayment_Throws_RequestValidationException_For_Unknown_Merchant()
        {
            //Arrange
            var merchantApiKey = Guid.NewGuid().ToString();
            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            var request = new ProcessPaymentRequest
            {
                CardCvv = "123",
                ExpiryDate = "2020-25",
                PaymentAmount = 15234.00m,
                Currency = "USD",
                CardNumber = "123456794"

            };
            try
            {
                //Act
                paymentService.ProcessPayment(request, merchantApiKey);
            }
            catch (RequestValidationException ex)
            {
                // Assert
                Assert.AreEqual("Unknown Merchant", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RequestValidationException))]
        public void Method_ProcessPayment_Throws_RequestValidationException_For_Missing_CardNumber()
        {
            //Arrange
            var merchantApiKey = Guid.NewGuid().ToString();
            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();
                       merchantDaoMock.Setup(x => x.GetMerchantBasedOnApiKey(It.IsAny<string>())).Returns(new Merchant());

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            var request = new ProcessPaymentRequest
            {
                CardCvv = "123",
                ExpiryDate = "2020-25",
                PaymentAmount = 15234.00m,
                Currency = "USD",
                CardNumber = string.Empty

            };
            try
            {
                //Act
                paymentService.ProcessPayment(request, merchantApiKey);
            }
            catch (RequestValidationException ex)
            {
                // Assert
                Assert.AreEqual("Missing Card Number", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RequestValidationException))]
        public void Method_ProcessPayment_Throws_RequestValidationException_For_Invalid_CardNumber()
        {
            //Arrange
            var merchantApiKey = Guid.NewGuid().ToString();
            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();
                       merchantDaoMock.Setup(x => x.GetMerchantBasedOnApiKey(It.IsAny<string>())).Returns(new Merchant());

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            var request = new ProcessPaymentRequest
            {
                CardCvv = "123",
                ExpiryDate = "2020-25",
                PaymentAmount = 15234.00m,
                Currency = "USD",
                CardNumber = "45123"

            };
            try
            {
                //Act
                paymentService.ProcessPayment(request, merchantApiKey);
            }
            catch (RequestValidationException ex)
            {
                // Assert
                Assert.AreEqual("Invalid Card Number", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RequestValidationException))]
        public void Method_ProcessPayment_Throws_RequestValidationException_For_Missing_CardCvv_Number()
        {
            //Arrange
            var merchantApiKey = Guid.NewGuid().ToString();
            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();
                       merchantDaoMock.Setup(x => x.GetMerchantBasedOnApiKey(It.IsAny<string>())).Returns(new Merchant());

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            var request = new ProcessPaymentRequest
            {
                CardCvv = string.Empty,
                ExpiryDate = "2020-25",
                PaymentAmount = 15234.00m,
                Currency = "USD",
                CardNumber = "1234567891234567"

            };
            try
            {
                //Act
                paymentService.ProcessPayment(request, merchantApiKey);
            }
            catch (RequestValidationException ex)
            {
                // Assert
                Assert.AreEqual("Missing Card CVV Number", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RequestValidationException))]
        public void Method_ProcessPayment_Throws_RequestValidationException_For_Invalid_CardCvv_Number()
        {
            //Arrange
            var merchantApiKey = Guid.NewGuid().ToString();
            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();
                       merchantDaoMock.Setup(x => x.GetMerchantBasedOnApiKey(It.IsAny<string>())).Returns(new Merchant());

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            var request = new ProcessPaymentRequest
            {
                CardCvv = "123",
                ExpiryDate = "2020-25",
                PaymentAmount = 15234.00m,
                Currency = "USD",
                CardNumber = "1234567891234567"

            };
            try
            {
                //Act
                paymentService.ProcessPayment(request, merchantApiKey);
            }
            catch (RequestValidationException ex)
            {
                // Assert
                Assert.AreEqual("Invalid Card Card CVV Number", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RequestValidationException))]
        public void Method_ProcessPayment_Throws_RequestValidationException_For_Invalid_Payment_Amount()
        {
            //Arrange
            var merchantApiKey = Guid.NewGuid().ToString();
            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();
                       merchantDaoMock.Setup(x => x.GetMerchantBasedOnApiKey(It.IsAny<string>())).Returns(new Merchant());

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            var request = new ProcessPaymentRequest
            {
                CardCvv = "1234",
                ExpiryDate = "2020-25",
                PaymentAmount = 0m,
                Currency = "USD",
                CardNumber = "1234567891234567"

            };
            try
            {
                //Act
                paymentService.ProcessPayment(request, merchantApiKey);
            }
            catch (RequestValidationException ex)
            {
                // Assert
                Assert.AreEqual("Payment Amount should be greater than zero", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RequestValidationException))]
        public void Method_ProcessPayment_Throws_RequestValidationException_For_Missing_Expiry_Date()
        {
            //Arrange
            var merchantApiKey = Guid.NewGuid().ToString();
            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();
                       merchantDaoMock.Setup(x => x.GetMerchantBasedOnApiKey(It.IsAny<string>())).Returns(new Merchant());

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            var request = new ProcessPaymentRequest
            {
                CardCvv = "1234",
                ExpiryDate = string.Empty,
                PaymentAmount = 1m,
                Currency = "USD",
                CardNumber = "1234567891234567"

            };
            try
            {
                //Act
                paymentService.ProcessPayment(request, merchantApiKey);
            }
            catch (RequestValidationException ex)
            {
                // Assert
                Assert.AreEqual("Missing Card Card Expiry Date", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RequestValidationException))]
        public void Method_ProcessPayment_Throws_RequestValidationException_For_Invalid_Expiry_Date()
        {
            //Arrange
            var merchantApiKey = Guid.NewGuid().ToString();
            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();
                       merchantDaoMock.Setup(x => x.GetMerchantBasedOnApiKey(It.IsAny<string>())).Returns(new Merchant());

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            var request = new ProcessPaymentRequest
            {
                CardCvv = "1234",
                ExpiryDate = "2020/01",
                PaymentAmount = 1m,
                Currency = "USD",
                CardNumber = "1234567891234567"

            };
            try
            {
                //Act
                paymentService.ProcessPayment(request, merchantApiKey);
            }
            catch (RequestValidationException ex)
            {
                // Assert
                Assert.AreEqual("Invalid ExpiryDate provided", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RequestValidationException))]
        public void Method_ProcessPayment_Throws_RequestValidationException_For_Missing_Currency()
        {
            //Arrange
            var merchantApiKey = Guid.NewGuid().ToString();
            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();
                       merchantDaoMock.Setup(x => x.GetMerchantBasedOnApiKey(It.IsAny<string>())).Returns(new Merchant());

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            var request = new ProcessPaymentRequest
            {
                CardCvv = "1234",
                ExpiryDate = "2020-01",
                PaymentAmount = 1m,
                Currency = string.Empty,
                CardNumber = "1234567891234567"

            };
            try
            {
                //Act
                paymentService.ProcessPayment(request, merchantApiKey);
            }
            catch (RequestValidationException ex)
            {
                // Assert
                Assert.AreEqual("Missing Currency", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RequestValidationException))]
        public void Method_ProcessPayment_Throws_RequestValidationException_For_Unknown_Currency()
        {
            //Arrange
            var merchantApiKey = Guid.NewGuid().ToString();
            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();
            merchantDaoMock.Setup(x => x.GetMerchantBasedOnApiKey(It.IsAny<string>())).Returns(new Merchant());
           

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            var request = new ProcessPaymentRequest
            {
                CardCvv = "1234",
                ExpiryDate = "2020-01",
                PaymentAmount = 1m,
                Currency = "CUR",
                CardNumber = "1234567891234567"

            };
            try
            {
                //Act
                paymentService.ProcessPayment(request, merchantApiKey);
            }
            catch (RequestValidationException ex)
            {
                // Assert
                Assert.AreEqual("Unknown Currency", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void Method_ProcessPayment_Returns_Payment_Identifier_And_Status_For_Valid_Request()
        {
            //Arrange
            var merchantApiKey = Guid.NewGuid().ToString();
            var paymentIdentifier = Guid.NewGuid();

            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();
            merchantDaoMock.Setup(x => x.GetMerchantBasedOnApiKey(It.IsAny<string>())).Returns(new Merchant{Name = "Test Merchant",ID = 1});

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            cardDaoMock.Setup(x => x.GetCardBasedOnNumberAndCVV(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new Card());
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            currencyDaoMock.Setup(x => x.GetCurrencyBasedOnShortDescription(It.IsAny<string>()))
                .Returns(new Currency());
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            paymentRequestDao
                .Setup(x => x.GetPaymentRequestBasedOnMerchantIdCardIdCurrencyIdAndAmount(It.IsAny<PaymentRequest>()))
                .Returns(new PaymentRequest
                {
                    MerchantId = 1,
                    CardId = 1
                });
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            acquiringBankServiceMock.Setup(x => x.ProcessPayment(It.IsAny<AcquiringBankRequest>())).Returns(
                new AcquiringBankResponse
                {
                    PaymentIdentifier = paymentIdentifier,
                    PaymentStatus = "Successful"
                });
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            var request = new ProcessPaymentRequest
            {
                CardCvv = "1234",
                ExpiryDate = "2020-01",
                PaymentAmount = 1m,
                Currency = "USD",
                CardNumber = "1234567891234567"

            };
            
            //Act
            var result =  paymentService.ProcessPayment(request, merchantApiKey);

            //Assert
            Assert.AreEqual(paymentIdentifier, result.PaymentUniqueId);
            Assert.AreEqual("Successful", result.Status);
        }

        [TestMethod]
        public void Method_GetPaymentDetail_Returns_Null_For_Unknown_Bank_Payment_Identifier()
        {
              //Arrange
            var paymentIdentifier = Guid.NewGuid();

            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);

            //Act
            var result = paymentService.GetPaymentDetail(paymentIdentifier.ToString());

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Method_GetPaymentDetail_Returns_Payment_Details_For_Valid_Bank_Payment_Identifier()
        {
            //Arrange
            var paymentIdentifier = Guid.NewGuid();
            var cardNumber = "1234567891234567";

            Mock<IMerchantDao> merchantDaoMock = new Mock<IMerchantDao>();

            Mock<ICardDao> cardDaoMock = new Mock<ICardDao>();
            Mock<ICurrencyDao> currencyDaoMock = new Mock<ICurrencyDao>();
            Mock<IPaymentRequestDao> paymentRequestDao = new Mock<IPaymentRequestDao>();
            Mock<IAcquiringBankService> acquiringBankServiceMock = new Mock<IAcquiringBankService>();
            Mock<IBankPaymentResponseDao> paymentResponseDaoMock = new Mock<IBankPaymentResponseDao>();
            paymentResponseDaoMock.Setup(s => s.GetBankPaymentResponse(It.IsAny<Guid>())).Returns(new PaymentDetail
            {
                CardNumber = cardNumber,
                Currency = "USD",
                PaymentStatus = "Successful",
                PaymentAmount = 1324.00m,
                CardCVV = "1234",
                PaymentIdentifier = paymentIdentifier
            });
            var paymentService = new PaymentService(cardDaoMock.Object, currencyDaoMock.Object, paymentRequestDao.Object,
                merchantDaoMock.Object, acquiringBankServiceMock.Object, paymentResponseDaoMock.Object);


            //Act
            var result = paymentService.GetPaymentDetail(paymentIdentifier.ToString());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cardNumber, DecryptCardNumber(result.CardNumber));
            
        }

        private string DecryptCardNumber(string DecryptText)

        {
            var key = ConfigurationManager.AppSettings["EncryptionKey"];

            byte[] SrctArray;

            byte[] DrctArray = Convert.FromBase64String(DecryptText);

            SrctArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();

            MD5CryptoServiceProvider objmdcript = new MD5CryptoServiceProvider();

            SrctArray = objmdcript.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            objmdcript.Clear();

            objt.Key = SrctArray;

            objt.Mode = CipherMode.ECB;

            objt.Padding = PaddingMode.PKCS7;

            ICryptoTransform crptotrns = objt.CreateDecryptor();

            byte[] resArray = crptotrns.TransformFinalBlock(DrctArray, 0, DrctArray.Length);

            objt.Clear();

            return UTF8Encoding.UTF8.GetString(resArray);

        }
    }
}
