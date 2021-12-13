using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Gateways
{
    public class ContactDetailsGateway : IContactDetailsGateway
    {
        private readonly IHousingData<List<ContactDetail>> _housingData;

        public ContactDetailsGateway(IHousingData<List<ContactDetail>> housingData)
        {
            _housingData = housingData;
        }
        public async Task<List<ContactDetail>> GetByTargetId(Guid targetId)
        {
            if (targetId == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(targetId).ToString()} shouldn't be empty or null");

            return await _housingData.DownloadAsync(targetId).ConfigureAwait(false);
        }
    }
}
