using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Response;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetAccountByIdUseCase : IGetAccountByIdUseCase
    {
        private readonly IAccountGateway _accountGateway;
        public GetAccountByIdUseCase(IAccountGateway gateway)
        {
            _accountGateway = gateway;
        }

        public async Task<AccountResponse> ExecuteAsync(Guid id)
        {
            if (id == Guid.Empty || id == null)
                throw new Exception("The id shouldn't be empty or null.");
            return await _accountGateway.GetById(id).ConfigureAwait(false);
        }
    }
}
