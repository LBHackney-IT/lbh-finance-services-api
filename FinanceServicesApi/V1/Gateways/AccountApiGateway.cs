using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Gateways
{
    public class AccountApiGateway : IAccountGateway
    {
        private readonly IHousingData<Account> _accountHD;
        private readonly IHousingData<List<Account>> _accountsHD;
        public AccountApiGateway(IHousingData<Account> accountHD, IHousingData<List<Account>> accountsHD)
        {
            _accountHD = accountHD;
            _accountsHD = accountsHD;
        }

        public async Task<Account> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(id)} shouldn't be empty.");
            }

            return await _accountHD.DownloadAsync(id).ConfigureAwait(false);
        }

        public async Task<Account> GetByTargetId(Guid targetId)
        {
            if (targetId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(targetId)} shouldn't be empty.");
            }

            var accounts = await _accountsHD.DownloadAsync(targetId).ConfigureAwait(false);

            return accounts.FirstOrDefault();
        }
    }
}
