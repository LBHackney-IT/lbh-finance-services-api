using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.HousingSearch.Domain.Transactions;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetTransactionByTargetIdUseCase : IGetTransactionByTargetIdUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetTransactionByTargetIdUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<List<Transaction>> ExecuteAsync(Guid targetId)
        {
            if (targetId == Guid.Empty)
                throw new ArgumentNullException(nameof(targetId));

            TransactionsRequest request = new TransactionsRequest
            {
                TargetId = targetId,
                SortBy = nameof(Transaction.TransactionDate),
                Page = 1,
                PageSize = 12,
                IsDesc = true
            };

            List<Transaction> transactions;
            do
            {
                transactions = await _gateway.GetByTargetId(request).ConfigureAwait(false);
                request.Page++;
            } while (transactions.Count == 0 || transactions.Any(p => p.PaidAmount > 0));

            return transactions;
        }
    }
}
