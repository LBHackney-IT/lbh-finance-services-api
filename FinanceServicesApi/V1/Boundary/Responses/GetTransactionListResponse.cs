using System;
using System.Collections.Generic;
using FinanceServicesApi.V1.Domain;
using FinanceServicesApi.V1.Domain.TransactionModels;

namespace FinanceServicesApi.V1.Boundary.Responses
{
    public class GetTransactionListResponse
    {
        public long Total { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
