using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Response;

namespace FinanceServicesApi.V1.UseCase.Interfaces.SuspenseTransaction
{
    public interface IGetTransactionByIdUseCase
    {
        public Task<TransactionResponse> ExecuteAsync(Guid id);
    }
}
