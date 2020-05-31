using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Payment.Gateway.Data.Merchant
{
    public class MerchantDao : IMerchantDao
    {
        private readonly string _connectionString;

        public MerchantDao()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["paymentGateway"].ConnectionString;
        }
        public Merchant GetMerchantBasedOnApiKey(string apiKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Merchant>(@"
DECLARE @ApiKey NVARCHAR(36) = @apiKeyIn

SELECT 
    ID, 
    Name, 
    Description, 
    ApiKey
FROM 
    Merchant
WHERE 
    ApiKey = @ApiKey
",new
                {
                    apiKeyIn = apiKey
                });
            }
        }
    }
}
