using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Domain.AccountModels
{
    public class AccountBalanceUpdateModel
    {
        public string value { get; set; }
        public string path { get; set; }
        public string op { get; set; }
    }
}
