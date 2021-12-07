using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackney.Shared.HousingSearch.Domain.Transactions;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetTransactionByIdUseCase
    {
        public Task<Transaction> ExecuteAsync(Guid id);
    }
}
