using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Gateways
{
    public class TenureInformationGateway : ITenureInformationGateway
    {
        private readonly IFinanceDomainApiData<TenureInformation> _housingData;

        public TenureInformationGateway(IFinanceDomainApiData<TenureInformation> housingData)
        {
            _housingData = housingData;
        }
        public async Task<TenureInformation> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"{nameof(id).ToString()} shouldn't be empty.");
            return await _housingData.DownloadAsync(id).ConfigureAwait(false);
        }
    }
}
