using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.HousingSearch.Domain.Person;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Gateways
{
    public class PersonGateway: IPersonGateway
    {
        private readonly ICustomeHttpClient _client;
        private readonly IGetEnvironmentVariables _getEnvironmentVariables;

        public PersonGateway(ICustomeHttpClient client,IGetEnvironmentVariables getEnvironmentVariables)
        {
            _client = client;
            _getEnvironmentVariables = getEnvironmentVariables;
        }
        public async Task<Person> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(id).ToString()} shouldn't be empty or null");

            var personApiUrl = _getEnvironmentVariables.GetAccountApiUrl().ToString();
            var personApiToken = _getEnvironmentVariables.GetAccountApiToken();

            _client.AddAuthorization(new AuthenticationHeaderValue("Bearer", personApiToken));

            var response = await _client.GetAsync(new Uri($"{personApiUrl}/api/v1/persons/{id.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception($"The person api is not reachable!{personApiUrl}");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var personResponse = JsonConvert.DeserializeObject<Person>(responseContent);
            return personResponse;
        }
    }
}
