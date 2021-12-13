using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Gateways
{
    public class TenureInformationGateway : ITenureInformationGateway
    {
        private readonly IHousingData<TenureInformation> _housingData;

        public TenureInformationGateway(IHousingData<TenureInformation> housingData)
        {
            _housingData = housingData;
        }
        public async Task<TenureInformation> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(id).ToString()} shouldn't be empty or null");
            return await _housingData.DownloadAsync(id).ConfigureAwait(false);
        }
    }
}
