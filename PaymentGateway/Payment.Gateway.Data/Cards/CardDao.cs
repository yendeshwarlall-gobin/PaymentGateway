using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Payment.Gateway.Data.Cards
{
    public class CardDao : ICardDao
    {
        private readonly string _connectionString;

        public CardDao()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["paymentGateway"].ConnectionString;
        }

        public Card GetCardBasedOnNumberAndCVV(string cardNumber, string cardCvv)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Card>(@"
DECLARE @CardNumber NVARCHAR(19) = @cardNumberIn
DECLARE @cardCvv NVARCHAR(19) = @cardCvvIn

SELECT 
    ID, 
    Number, 
    CVV, 
    ExpiryDate
FROM 
    Card
WHERE 
    Number =@CardNumber
AND CVV = @cardCvv
", new
                {
                    cardNumberIn = cardNumber, 
                    cardCvvIn = cardCvv
                });
            }
        }

        public void InsertCardInfo(Card newCard)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(@"
DECLARE @CardNumber NVARCHAR(19) = @cardNumberIn
DECLARE @Cvv NVARCHAR(19) = @cvvIn
DECLARE @ExpiryDate DateTime = @expiryDateIn

INSERT INTO [dbo].[Card]
           ([Number]
           ,[CVV]
           ,[ExpiryDate])
     VALUES
           (@CardNumber
           ,@Cvv
           ,@ExpiryDate)
", new
                {
                    cardNumberIn = newCard.Number,
                    cvvIn = newCard.CVV, 
                    expiryDateIn = newCard.ExpiryDate
                });
            }
        }

        public Card GetCardById(int cardId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Card>(@"
DECLARE @CardId int = @cardIdIn

SELECT 
    ID, 
    Number, 
    CVV, 
    ExpiryDate
FROM 
    Card
WHERE 
    Id = @CardId
", new
                {
                    cardIdIn = cardId
                });
            }
        }
    }
}
