using System;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class GetAccountEnvironmentVariables : IGetEnvironmentVariables<Account>
    {
        public Uri GetUrl()
        {
            string result = Environment.GetEnvironmentVariable("ACCOUNT_API_URL") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Account api url shouldn't be null or empty.");
            return new Uri(result);
        }

        public string GetToken()
        {
            string result = Environment.GetEnvironmentVariable("ACCOUNT_API_TOKEN") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Account api token shouldn't be null or empty.");
            return result;
        }
    }
}
