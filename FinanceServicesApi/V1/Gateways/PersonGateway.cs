using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Person;

namespace FinanceServicesApi.V1.Gateways
{
    public class PersonGateway : IPersonGateway
    {
        private readonly IHousingData<Person> _housingData;

        public PersonGateway(IHousingData<Person> housingData)
        {
            _housingData = housingData;
        }

        public async Task<Person> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(id).ToString()} shouldn't be empty or null");
            return await _housingData.DownloadAsync(id).ConfigureAwait(false);
        }
    }
}
