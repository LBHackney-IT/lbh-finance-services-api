using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Asset.Domain;

namespace FinanceServicesApi.V1.Gateways
{
    public class AssetGateway : IAssetGateway
    {
        private readonly IHousingData<Asset> _housingData;

        public AssetGateway(IHousingData<Asset> housingData)
        {
            _housingData = housingData;
        }
        public async Task<Asset> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"{nameof(id).ToString()} shouldn't be empty.");
            return await _housingData.DownloadAsync(id).ConfigureAwait(false);
        }
    }
}
