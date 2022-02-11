using System;
using System.Collections.Generic;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure.Environments
{
    public class GetTransactionsEnvironmentVariable : IGetEnvironmentVariables<List<Transaction>>
    {
        public Uri GetUrl()
        {
            string result = Environment.GetEnvironmentVariable("TRANSACTION_API_URL") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Transaction api url shouldn't be null or empty.");
            return new Uri(result);
        }
    }
}
