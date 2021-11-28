using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackney.Shared.HousingSearch.Domain.Transactions;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetTransactionByTargetIdUseCase
    {
        public Task<List<Transaction>> ExecuteAsync(Guid targetId);
    }
}
