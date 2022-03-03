using System.Collections.Generic;
using FinanceServicesApi.V1.Domain.AccountModels;

namespace FinanceServicesApi.V1.Boundary.Responses
{
    public class GetAccountListResponse
    {
        public List<AccountResponseList> AccountResponseList { get; set; }
    }

    public class AccountResponseList : Account
    {

    }
}
