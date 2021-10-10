using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Response;

namespace FinanceServicesApi.V1.Gateways.Interfaces.SuspenseTransaction
{
    public interface IAccountGateway
    {
        public Task<AccountResponse> GetById(Guid id);
    }
}