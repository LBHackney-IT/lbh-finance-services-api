using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.AccountModels;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface IAccountGateway
    {
        public Task<Account> GetById(Guid id);
    }
}
