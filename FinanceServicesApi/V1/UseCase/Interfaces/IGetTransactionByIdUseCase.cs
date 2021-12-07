using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.TransactionModels;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetTransactionByIdUseCase
    {
        public Task<Transaction> ExecuteAsync(Guid id);
    }
}
