using System;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure.Environments
{
    public class GetAccountsEnvironmentVariables : IGetEnvironmentVariables<GetAccountListResponse>
    {
        public Uri GetUrl()
        {
            string result = Environment.GetEnvironmentVariable("ACCOUNT_API_URL") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Account api url shouldn't be null or empty.");
            return new Uri(result);
        }
    }
}
