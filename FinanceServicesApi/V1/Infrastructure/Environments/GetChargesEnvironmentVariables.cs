using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;

namespace FinanceServicesApi.V1.Infrastructure.Environments
{
    public class GetChargesEnvironmentVariables : IGetEnvironmentVariables<List<Charge>>
    {
        public Uri GetUrl()
        {
            string result = Environment.GetEnvironmentVariable("CHARGE_API_URL") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Changes api url shouldn't be null or empty.");
            return new Uri(result);
        }
    }
}
