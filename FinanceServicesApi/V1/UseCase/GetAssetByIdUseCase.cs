using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Asset.Domain;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetAssetByIdUseCase : IGetAssetByIdUseCase
    {
        private readonly IAssetGateway _gateway;

        public GetAssetByIdUseCase(IAssetGateway gateway)
        {
            _gateway = gateway;
        }
        public async Task<Asset> ExecuteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException($"{nameof(id).ToString()} cannot be empty.");
            return await _gateway.GetById(id).ConfigureAwait(false);
        }
    }
}
