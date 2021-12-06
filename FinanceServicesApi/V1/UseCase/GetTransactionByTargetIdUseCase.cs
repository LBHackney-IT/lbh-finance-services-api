using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            if (targetId== null)
                throw new Exception("The id shouldn't be empty or null.");
            return await _gateway.GetByTargetId(targetId).ConfigureAwait(false);
        }
    }
}
