using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetAccountByTargetIdUseCase : IGetAccountByTargetIdUseCase
    {
        private readonly IAccountGateway _gateway;

        public GetAccountByTargetIdUseCase(IAccountGateway gateway)
        {
            _gateway = gateway;
        }
        public async Task<Account> ExecuteAsync(Guid targetId)
        {
            if (targetId == Guid.Empty)
                throw new NullReferenceException(nameof(targetId));
            return await _gateway.GetByTargetId(targetId).ConfigureAwait(false);
        }
    }
}
