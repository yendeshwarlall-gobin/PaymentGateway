using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Payment.Gateway.Data.Currency
{
    public class CurrencyDao : ICurrencyDao
    {
        private readonly string _connectionString;

        public CurrencyDao()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["paymentGateway"].ConnectionString;
        }

        public Currency GetCurrencyBasedOnDescription(string description)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Currency>(@"
DECLARE @Description NVARCHAR(100) = @DescriptionIn

SELECT 
    ID, 
    Description, 
    ShortDescription
FROM 
    Currency
WHERE 
    Description = @Description
", new
                {
                    DescriptionIn = description
                });
            }
        }

        public Currency GetCurrencyBasedOnShortDescription(string shortDescription)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Currency>(@"
DECLARE @ShortDescription char(3) = @ShortDescriptionIn

SELECT 
    ID, 
    Description, 
    ShortDescription
FROM 
    Currency
WHERE 
    ShortDescription = @ShortDescription
", new
                {
                    ShortDescriptionIn = shortDescription
                });
            }
        }
    }
}
