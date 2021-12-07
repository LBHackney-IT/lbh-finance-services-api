using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackney.Shared.HousingSearch.Domain.Transactions;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetLastPaymentTransactionsByTargetIdUseCase
    {
        public Task<List<Transaction>> ExecuteAsync(Guid targetId);
    }
}
