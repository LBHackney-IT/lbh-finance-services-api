using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Response;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface ITransactionGateway
    {
        public Task<TransactionResponse> GetById(Guid id);
    }
}
