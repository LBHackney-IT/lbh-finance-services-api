using System;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure
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

        public string GetTenureInformationApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("TENURE_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("tenure api url shouldn't be null or empty");
            return result;
        }

        public string GetTenureInformationApiToken()
        {
            string result = Environment.GetEnvironmentVariable("TENURE_API_TOKEN") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("tenure api key shouldn't be null or empty");
            return result;
        }

        public string GetFinancialSummaryApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("FINANCIAL_SUMMARY_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("financial summary api url shouldn't be null or empty");
            return result;
        }

        public string GetFinancialSummaryApiKey()
        {
            string result = Environment.GetEnvironmentVariable("FINANCIAL_SUMMARY_API_KEY") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("financial summary api key shouldn't be null or empty");
            return result;
        }
    }
}
