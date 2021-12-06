using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.HousingSearch.Domain.Accounts;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetAccountByIdUseCase : IGetAccountByIdUseCase
    {
        private readonly IAccountGateway _accountGateway;
        public GetAccountByIdUseCase(IAccountGateway gateway)
        {
            _accountGateway = gateway;
        }

        public async Task<Account> ExecuteAsync(Guid id)
        {
            return await _accountGateway.GetById(id).ConfigureAwait(false);
        }
    }
}
