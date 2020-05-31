
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Payment.Gateway.Data.PaymentRequest
{
    public class PaymentRequestDao : IPaymentRequestDao
    {
        private readonly string _connectionString;

        public PaymentRequestDao()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["paymentGateway"].ConnectionString;
        }

        public void InsertPaymentRequest(PaymentRequest request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(@"
DECLARE @MerchantId int = @MerchantIdIn
DECLARE @CardId int = @CardIdIn
DECLARE @CurrencyId int = @CurrencyIdIn
DECLARE @Amount decimal(20,4) = @AmountIn
DECLARE @DateTimeAdded datetime = @DateTimeAddedIn

INSERT INTO [dbo].[PaymentRequest]
           ([MerchantId]
           ,[CardId]
           ,[Amount]
           ,[CurrencyId]
           ,[DateTimeAdded])
     VALUES
           (@MerchantId
           ,@CardId
           ,@Amount
           ,@CurrencyId 
           ,@DateTimeAdded)
", new
                {
                    MerchantIdIn = request.MerchantId,
                    CardIdIn = request.CardId,
                    CurrencyIdIn = request.CurrencyId,
                    AmountIn = request.Amount,
                    DateTimeAddedIn = request.DateTimeAdded
                });
            }
        }

        public PaymentRequest GetPaymentRequestBasedOnMerchantIdCardIdCurrencyIdAndAmount(PaymentRequest request)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return connection.QueryFirstOrDefault<PaymentRequest>(@"
DECLARE @MerchantId int = @MerchantIdIn
DECLARE @CardId int = @CardIdIn
DECLARE @CurrencyId int = @CurrencyIdIn
DECLARE @Amount decimal(20,4) = @AmountIn

SELECT 
     [Id]
    ,[MerchantId]
    ,[CardId]
    ,[Amount]
    ,[CurrencyId]
    ,[DateTimeAdded]
FROM 
    [dbo].[PaymentRequest]
WHERE 
    MerchantId = @MerchantId
AND CardId = @CardId
AND CurrencyId = @CurrencyId
AND Amount = @Amount
", new
                    {
                        MerchantIdIn = request.MerchantId,
                        CardIdIn = request.CardId,
                        CurrencyIdIn = request.CurrencyId,
                        AmountIn = request.Amount,
                    });
                }
            }
    }
}
