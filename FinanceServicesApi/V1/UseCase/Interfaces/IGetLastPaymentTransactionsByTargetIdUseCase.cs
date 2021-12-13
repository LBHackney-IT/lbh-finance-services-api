using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.TransactionModels;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetLastPaymentTransactionsByTargetIdUseCase
    {
        public Task<List<Transaction>> ExecuteAsync(Guid targetId);
    }
}
