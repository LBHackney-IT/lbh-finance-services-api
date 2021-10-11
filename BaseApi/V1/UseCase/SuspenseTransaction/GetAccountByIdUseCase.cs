using System;
using System.Threading.Tasks;
using BaseApi.V1.Boundary.Response;
using BaseApi.V1.Gateways.Interfaces.SuspenseTransaction;
using BaseApi.V1.UseCase.Interfaces.SuspenseTransaction;

namespace BaseApi.V1.UseCase.SuspenseTransaction
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
