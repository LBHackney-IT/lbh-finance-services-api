using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Gateways
{
    public class ChargesApiGateway : IChargesGateway
    {
        private readonly IHousingData<List<Charge>> _housingData;

        public ChargesApiGateway(IHousingData<List<Charge>> housingData)
        {
            _housingData = housingData;
        }

        public async Task<List<Charge>> GetAllByAssetId(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(assetId));
            }

            return await _housingData.DownloadAsync(assetId).ConfigureAwait(false);
        }
    }
}
