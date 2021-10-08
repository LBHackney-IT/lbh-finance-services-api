using System;
using System.Threading.Tasks;
using BaseApi.V1.Infrastructure.Interfaces;

namespace BaseApi.V1.Infrastructure
{
    public class EnvironmentVariables:IEnvironmentVariables
    {
        public string GetAccountApiUrl()
        {
            return Environment.GetEnvironmentVariable("ACCOUNT_API_URL") ?? string.Empty;
        }

        public string GetAccountApiToken()
        {
            return Environment.GetEnvironmentVariable("ACCOUNT_API_TOKEN") ?? string.Empty; ;
        }

        public string GetTransactionApiUrl()
        {
            return Environment.GetEnvironmentVariable("TRANSACTION_API_URL") ?? string.Empty;
        }

        public string GetTransactionApiKey()
        {
            return Environment.GetEnvironmentVariable("TRANSACTION_API_KEY") ?? string.Empty; ;
        }
    }
}
