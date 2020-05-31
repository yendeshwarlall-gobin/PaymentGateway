using System;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Payment.Gateway.Data.BankPaymentResponse
{
    public class BankPaymentResponseDao:IBankPaymentResponseDao
    {
        private readonly string _connectionString;

        public BankPaymentResponseDao()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["paymentGateway"].ConnectionString;
        }
        public void InsertBankPaymentResponse(BankPaymentResponse bankPaymentResponse)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(@"
DECLARE @PaymentRequestId int = @PaymentRequestIdIn
DECLARE @BankPaymentIdentifier uniqueIdentifier = @BankPaymentIdentifierIn
DECLARE @Status nvarchar(255) = @StatusIn
DECLARE @DateTimeAdded datetime = @DateTimeAddedIn

INSERT INTO [dbo].[BankPaymentResponse]
           ([PaymentRequestId]
           ,[BankPaymentIdentifier]
           ,[Status]
           ,[DateTimeAdded])
     VALUES
           (@PaymentRequestId
           ,@BankPaymentIdentifier
           ,@Status
           ,@DateTimeAdded)
", new
                {
                    PaymentRequestIdIn = bankPaymentResponse.PaymentRequestId,
                    BankPaymentIdentifierIn = bankPaymentResponse.BankPaymentIdentifier,
                    StatusIn = bankPaymentResponse.Status,
                    DateTimeAddedIn = bankPaymentResponse.DateTimeAdded,
                });
            }
        }

        public PaymentDetail GetBankPaymentResponse(Guid bankPaymentIdentifier)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
              return  connection.QueryFirstOrDefault<PaymentDetail>(@"
DECLARE @BankPaymentIdentifier uniqueIdentifier = @BankPaymentIdentifierIn

SELECT 
     bpr.BankPaymentIdentifier AS PaymentIdentifier
    ,c.Number AS CardNumber
    ,c.CVV AS CardCVV
    ,cur.Description AS Currency
    ,pr.Amount AS PaymentAmount
    ,bpr.Status AS PaymentStatus
FROM BankPaymentResponse bpr
JOIN PaymentRequest pr on pr.Id = bpr.PaymentRequestId
JOIN Currency cur on cur.Id = pr.CurrencyId
JOIN Card c on c.Id = pr.CardId

WHERE 
    [BankPaymentIdentifier] = @BankPaymentIdentifier
", new{
                  BankPaymentIdentifierIn = bankPaymentIdentifier

              });
            }
        }
    }
}
