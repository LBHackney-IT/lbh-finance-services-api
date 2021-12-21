using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetChargeByAssetIdUseCase : IGetChargeByAssetIdUseCase
    {
        private readonly IChargesGateway _gateway;

        public GetChargeByAssetIdUseCase(IChargesGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<List<Charge>> ExecuteAsync(Guid assetId)
        {
            return await _gateway.GetAllByAssetId(assetId).ConfigureAwait(false);
        }
    }
}
