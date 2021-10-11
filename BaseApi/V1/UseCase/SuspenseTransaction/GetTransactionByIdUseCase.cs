using System;
using System.Threading.Tasks;
using BaseApi.V1.Boundary.Response;
using BaseApi.V1.Gateways.Interfaces.SuspenseTransaction;
using BaseApi.V1.UseCase.Interfaces.SuspenseTransaction;

namespace BaseApi.V1.UseCase.SuspenseTransaction
{
    public class GetTransactionByIdUseCase : IGetTransactionByIdUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetTransactionByIdUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public Task<TransactionResponse> ExecuteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception("The id shouldn't be empty");
            return _gateway.GetById(id);
        }
    }
}
