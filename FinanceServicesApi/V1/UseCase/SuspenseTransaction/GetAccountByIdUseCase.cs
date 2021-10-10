using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Response;
using FinanceServicesApi.V1.Gateways.Interfaces.SuspenseTransaction;
using FinanceServicesApi.V1.UseCase.Interfaces.SuspenseTransaction;

namespace FinanceServicesApi.V1.UseCase.SuspenseTransaction
{
    public class GetAccountByIdUseCase : IGetAccountByIdUseCase
    {
        private readonly IAccountGateway _accountGateway;
        public GetAccountByIdUseCase(IAccountGateway gateway)
        {
            _accountGateway = gateway;
        }

        public Task<AccountResponse> ExecuteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception("The id shouldn't be empty");
            return _accountGateway.GetById(id);
        }
    }
}