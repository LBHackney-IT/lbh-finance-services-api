using System;
using System.Collections.Generic;
using FinanceServicesApi.V1.Domain;
using FinanceServicesApi.V1.Domain.AccountModels;

namespace FinanceServicesApi.V1.Boundary.Responses
{
    public class GetAccountListResponse
    {
        public long Total { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
