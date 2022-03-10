using FinanceServicesApi.V1.Domain.TransactionModels;
using System;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetTransactionByIdUseCase
    {
        public Task<Transaction> ExecuteAsync(Guid id);
    }
}
