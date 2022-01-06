using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Gateways
{
    public class ContactDetailsGateway : IContactDetailsGateway

    {
        private readonly IHousingData<GetContactDetailsResponse> _housingData;

        public ContactDetailsGateway(IHousingData<GetContactDetailsResponse> housingData)
        {
            _housingData = housingData;
        }

        public async Task<GetContactDetailsResponse> GetByTargetId(Guid targetId)
        {
            if (targetId == Guid.Empty)
                throw new ArgumentException($"{nameof(targetId).ToString()} shouldn't be empty.");

            return await _housingData.DownloadAsync(targetId).ConfigureAwait(false);
        }
    }
}
