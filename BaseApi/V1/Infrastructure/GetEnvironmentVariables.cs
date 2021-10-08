using System;
using BaseApi.V1.Infrastructure.Interfaces;

namespace BaseApi.V1.Infrastructure
{
    public class GetEnvironmentVariables : IGetEnvironmentVariables
    {
        public string GetAccountApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("ACCOUNT_API_URL") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Account api url shouldn't be null or empty");
            return result;
        }

        public string GetAccountApiToken()
        {
            string result = Environment.GetEnvironmentVariable("ACCOUNT_API_TOKEN") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Account api token shouldn't be null or empty");
            return result;
        }

        public string GetTransactionApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("TRANSACTION_API_URL") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Transaction api url shouldn't be null or empty");
            return result;
        }

        public string GetTransactionApiKey()
        {
            string result = Environment.GetEnvironmentVariable("TRANSACTION_API_KEY") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Transaction api key shouldn't be null or empty");
            return result;
        }
    }
}
