using System;
using System.Threading.Tasks;
using BaseApi.V1.Boundary.Response;
using BaseApi.V1.Gateways.Interfaces.SuspenseTransaction.Account;
using BaseApi.V1.UseCase.Interfaces.SuspenseTransaction.Accounts;

namespace BaseApi.V1.UseCase.SuspenseTransaction.Account
{
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private readonly IAccountGateway _gateway;
        public GetByIdUseCase(IAccountGateway gateway)
        {
            _gateway = gateway;
        }

        public Task<AccountResponse> ExecuteAsync(Guid id)
        {
            return _gateway.GetEntityById(id);
        }
    }
}
