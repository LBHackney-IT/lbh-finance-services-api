using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetChargeByTargetIdUseCase : IGetChargeByTargetIdUseCase
    {
        private readonly IChargesGateway _gateway;

        public GetChargeByTargetIdUseCase(IChargesGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Charge> ExecuteAsync(Guid targetId, TargetType targetType)
        {
            return await _gateway.GetAllByTargetId(targetId, targetType).ConfigureAwait(false);
        }
    }
}
