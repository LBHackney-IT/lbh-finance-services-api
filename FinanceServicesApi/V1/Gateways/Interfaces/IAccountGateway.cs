using System;
using System.Threading.Tasks;
using Hackney.Shared.HousingSearch.Domain.Accounts;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface IAccountGateway
    {
        public Task<Account> GetById(Guid id);
    }
}
