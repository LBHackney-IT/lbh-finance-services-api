using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Gateways
{
    public class ContactDetailsGateway : IContactDetailsGateway
    {
        private readonly ICustomeHttpClient _client;
        private readonly IGetEnvironmentVariables _getEnvironmentVariables;

        public ContactDetailsGateway(ICustomeHttpClient client, IGetEnvironmentVariables getEnvironmentVariables)
        {
            _client = client;
            _getEnvironmentVariables = getEnvironmentVariables;
        }
        public async Task<List<ContactDetail>> GetByTargetId(Guid targetId)
        {
            if (targetId == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(targetId).ToString()} shouldn't be empty or null");

            var contactDetailsApiUrl = _getEnvironmentVariables.GetContactDetailsApiUrl();
            var contactDetailsApiToken = _getEnvironmentVariables.GetContactDetailsApiToken();

            _client.AddAuthorization(new AuthenticationHeaderValue("Bearer", contactDetailsApiToken));
            var response = await _client.GetAsync(new Uri($"{contactDetailsApiUrl}/contactDetails?TargetId={targetId.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The contact details api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contactDetailsResponse = JsonConvert.DeserializeObject<GetContactDetailsResponse>(responseContent);
            return contactDetailsResponse?.Results;
        }
    }
}
