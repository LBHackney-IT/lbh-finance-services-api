using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.HousingSearch.Domain.Transactions;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetTransactionByIdUseCase : IGetTransactionByIdUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetTransactionByIdUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Transaction> ExecuteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var transactions = await _gateway.GetById(id).ConfigureAwait(false);
            return transactions;
        }
    }
}
