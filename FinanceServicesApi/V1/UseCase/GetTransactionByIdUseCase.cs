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
            if (id == null)
                throw new Exception("The id shouldn't be empty or null.");
            return await _gateway.GetById(id).ConfigureAwait(false);
        }
    }
}
