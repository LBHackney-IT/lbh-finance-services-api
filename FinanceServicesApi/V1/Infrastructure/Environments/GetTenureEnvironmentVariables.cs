using System;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Infrastructure.Environments
{
    public class GetTenureEnvironmentVariables : IGetEnvironmentVariables<TenureInformation>
    {
        public Uri GetUrl()
        {
            string result = Environment.GetEnvironmentVariable("TENURE_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Tenure api url shouldn't be null or empty.");
            return new Uri(result);
        }
    }
}
