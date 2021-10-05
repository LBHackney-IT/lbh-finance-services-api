using System;
using System.Threading.Tasks;
using BaseApi.V1.Boundary.Response;

namespace BaseApi.V1.UseCase.Interfaces.SuspenseTransaction
{
    public interface IGetTransactionByIdUseCase
    {
        public Task<TransactionResponse> ExecuteAsync(Guid id);
    }
}
