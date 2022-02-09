using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Domain.AccountModels
{
    public class AccountPatchModel
    {
        public Guid Id { get; set; }
        public IList<AccountBalanceUpdateModel> Patch { get; set; }
    }
}
